using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;

namespace CarRental.WEB.Controllers
{
    public class CarController : Controller
    {
        readonly IRentService _rentService;
        public CarController(IRentService serv)
        {
            _rentService = serv;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars()));
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            return PartialView("Partials/_CarsList", mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars(search)));
        }

        [HttpGet]
        public ActionResult ShowFilters()
        {
            var carsDto = _rentService.GetCars();            

            var brands = new List<string>();
            var classes = new List<string>();
            
            foreach (var carDTO in carsDto)
            {
                if(!brands.Contains(carDTO.Brand))
                    brands.Add(carDTO.Brand);
                if (!classes.Contains(carDTO.Class))
                    classes.Add(carDTO.Class);
            }
            var filter = new FilterViewModel
            {
                AirConditioning = null,
                AutomaticTransmission = null,
                MaxPrice = null,
                MinPrice = null,
                Classes = classes.ToArray(),
                Brands = brands.ToArray()
            };
            return PartialView("Partials/_Filters", filter);
        }
        
        [HttpPost]
        public ActionResult Filter(FilterViewModel filterModel)
        {
            filterModel.MaxPrice = System.Convert.ToInt32(Request.Form["MaxPrice"].Split('.')[0]);
            filterModel.MinPrice = System.Convert.ToInt32(Request.Form["MinPrice"].Split('.')[0]);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarDTO, CarViewModel>();
                cfg.CreateMap<FilterViewModel, FilterDTO>();
            });
            var mapper = config.CreateMapper();
            var filterDTO = mapper.Map<FilterDTO>(filterModel);
            return PartialView("Partials/_CarsList", mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars(filterDTO)));
        }

        protected override void Dispose(bool disposing)

        {
            _rentService.Dispose();
            base.Dispose(disposing);
        }
    }
}