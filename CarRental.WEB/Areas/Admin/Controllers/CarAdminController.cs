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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarViewModel carView, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null)
            {
                byte[] imageData;
                // reads uploaded file to byte array
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                carView.Photo = imageData;
            }

            try
            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarViewModel carView)
        {
            try
            {
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