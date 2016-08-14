using System.Collections.Generic;
using System.Linq;
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
            var cars = mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars(search));
            Session["CurrentCars"] = cars;
            return PartialView("Partials/_CarsList", cars);
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
            var cars = mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars(filterDTO));
            Session["CurrentCars"] = cars;
            return PartialView("Partials/_CarsList", cars);
        }

        [HttpPost]
        public ActionResult SortByName(bool desc = false)
        {
            if (Session["CurrentCars"] == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();
                var carsView = mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars());
                if (desc)
                    return PartialView("Partials/_CarsList", carsView.OrderByDescending(x => x.Brand).ToList());
                return PartialView("Partials/_CarsList", carsView.OrderBy(x => x.Brand).ToList());
            }
            else
            {
                var carsView = (List<CarViewModel>)Session["CurrentCars"];
                if (desc)
                    return PartialView("Partials/_CarsList", carsView.OrderByDescending(x => x.Brand).ToList());
                return PartialView("Partials/_CarsList", carsView.OrderBy(x => x.Brand).ToList());
            }
        }

        [HttpPost]
        public ActionResult SortByPrice(bool desc = false)
        {
            if (Session["CurrentCars"] == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();
                var carsView = mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars());
                if (desc)
                    return PartialView("Partials/_CarsList", carsView.OrderByDescending(x => x.PriceForDay).ToList());
                return PartialView("Partials/_CarsList", carsView.OrderBy(x => x.PriceForDay).ToList());
            }
            else
            {
                var carsView = (List<CarViewModel>)Session["CurrentCars"];
                if (desc)
                    return PartialView("Partials/_CarsList", carsView.OrderByDescending(x => x.PriceForDay).ToList());
                return PartialView("Partials/_CarsList", carsView.OrderBy(x => x.PriceForDay).ToList());
            }
            
        }

        protected override void Dispose(bool disposing)

        {
            _rentService.Dispose();
            base.Dispose(disposing);
        }
    }
}