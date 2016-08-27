using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using NLog;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CarAdminController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly IRentService _rentService;

        public CarAdminController(IRentService serv)
        {
            _rentService = serv;
        }

        /// <summary>
        /// Showing the list of cars
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Admin/Cars page. User: {0}", User.Identity.Name);
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                return View(mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars()));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Request to Admin/Cars page failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Searching cars by model name and/or brand
        /// </summary>
        /// <param name="search">String for search</param>
        /// <returns>List of cars</returns>
        [HttpPost]
        public ActionResult Search(string search)
        {
            Logger.Debug("Admin search car request. Data: '{0}'", search);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            var cars = mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars(search));
            Session["CurrentAdminCars"] = cars;
            return PartialView("Partials/_CarsList", cars);
        }

        /// <summary>
        /// Shows view for creating a new car
        /// </summary>
        /// <returns>View for creating</returns>
        [HttpGet]
        public ActionResult Create()
        {
            Logger.Debug("Request to Admin/Car/Create page. User: {0}", User.Identity.Name);
            return View();
        }

        /// <summary>
        /// Creating a new car
        /// </summary>
        /// <param name="carView">Car model</param>
        /// <param name="uploadImage">Photo for model</param>
        /// <returns>If creating was done, redirected to 'Index' action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarViewModel carView, HttpPostedFileBase uploadImage)
        {
            Logger.Debug("Attempt to create a new car.");
            try
            {
                if (uploadImage != null)
                {
                    //validate file format
                    string[] fileNameArr = uploadImage.FileName.Split('.');
                    if(!fileNameArr[fileNameArr.Length-1].Equals("jpg") && !fileNameArr[fileNameArr.Length - 1].Equals("png") &&
                        !fileNameArr[fileNameArr.Length - 1].Equals("jpeg"))
                        throw new ValidationException("Wrong file format", "Photo");

                    byte[] imageData;
                    // reads uploaded file to byte array
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    carView.Photo = imageData;
                }
           
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CarViewModel, CarDTO>());
                var mapper = config.CreateMapper();
                _rentService.CreateCar(mapper.Map<CarDTO>(carView));

                Logger.Info("New car added by {0}. Car info (brand: {1}, model: {2}, class: {3}, price: {4})", 
                    User.Identity.Name, carView.Brand, carView.ModelName, carView.Class, carView.PriceForDay);

            }
            catch (ValidationException ex)
            {
                Logger.Debug("Car creating failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(carView);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Shows view for updating a car
        /// </summary>
        /// <param name="id">Car's id to update</param>
        /// <returns>View for editing</returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Logger.Debug("Request to Admin/Car/Edit page. User: {0}", User.Identity.Name);
            try
            {               
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CarDTO, CarViewModel>());
                var mapper = config.CreateMapper();
                return View(mapper.Map<CarViewModel>(_rentService.GetCar(id)));
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Request to Admin/Car/Edit page failed. Invalid id");
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Updating a car
        /// </summary>
        /// <param name="carView">Updated ar model</param>
        /// <param name="uploadImage">New car photo</param>
        /// <returns>If updating was done, redirected to 'Index' action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarViewModel carView, HttpPostedFileBase uploadImage)
        {
            Logger.Debug("Attempt to edit a car. Car id: {0}", carView.Id);
            try
            {
                if (uploadImage != null)
                {
                    //validate file format
                    string[] fileNameArr = uploadImage.FileName.Split('.');
                    if (!fileNameArr[fileNameArr.Length - 1].Equals("jpg") &&
                        !fileNameArr[fileNameArr.Length - 1].Equals("png") &&
                        !fileNameArr[fileNameArr.Length - 1].Equals("jpeg"))
                        throw new ValidationException("Wrong file format", "Photo");

                    byte[] imageData;
                    // reads uploaded file to byte array
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    carView.Photo = imageData;
                }
                else
                {
                    carView.Photo = _rentService.GetCar(carView.Id).Photo;
                }

                var config = new MapperConfiguration(cfg => cfg.CreateMap<CarViewModel, CarDTO>());
                var mapper = config.CreateMapper();
                _rentService.UpdateCar(mapper.Map<CarDTO>(carView));

                Logger.Info("The car #{5} edited by {0}. Car info (brand: {1}, model: {2}, class: {3}, price: {4}, air-conditioning: {6}, automate: {7}, seats: {8})",
                    User.Identity.Name, carView.Brand, carView.ModelName, carView.Class, carView.PriceForDay, carView.Id, 
                    carView.AirConditioning, carView.AutomaticTransmission, carView.Seats);

            }
            catch (ValidationException ex)
            {
                Logger.Debug("Car editing failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(carView);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deleting a car
        /// </summary>
        /// <param name="carId">Car's id to delete</param>
        /// <returns>List of cars</returns>
        [HttpPost]
        public ActionResult Delete(int? carId)
        {
            Logger.Debug("Attempt to delete a car. Car id: {0}", carId);
            try
            {
                _rentService.DeleteCar(carId);
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Car deleting failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
            }

            Logger.Info("Car #{0} was deleted by {1}.", carId, User.Identity.Name);

            if (Session["CurrentAdminCars"] == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();
                return PartialView("Partials/_CarsList", mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars()));
            }
            Session["CurrentAdminCars"] =
                ((List<CarViewModel>) Session["CurrentAdminCars"]).Select(x => x).Where(x => x.Id != carId).ToList();
            return PartialView("Partials/_CarsList", Session["CurrentAdminCars"]);
        }
    }
}