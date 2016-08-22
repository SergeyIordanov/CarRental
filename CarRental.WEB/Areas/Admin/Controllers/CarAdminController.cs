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

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CarAdminController : Controller
    {
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
            }
            catch (ValidationException ex)
            {
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
            try
            {               
                var config = new MapperConfiguration(cfg => cfg.CreateMap<CarDTO, CarViewModel>());
                var mapper = config.CreateMapper();
                return View(mapper.Map<CarViewModel>(_rentService.GetCar(id)));
            }
            catch (ValidationException ex)
            {
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
            try
            {
                if (uploadImage != null)
                {
                    //validate file format
                    string[] fileNameArr = uploadImage.FileName.Split('.');
                    if (!fileNameArr[fileNameArr.Length - 1].Equals("jpg") && !fileNameArr[fileNameArr.Length - 1].Equals("png") &&
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
                _rentService.UpdateCar(mapper.Map<CarDTO>(carView));
            }
            catch (ValidationException ex)
            {
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
            try
            {
                _rentService.DeleteCar(carId);                
            }
            catch (ValidationException){}
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