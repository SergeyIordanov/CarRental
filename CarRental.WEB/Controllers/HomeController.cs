using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;

namespace CarRental.WEB.Controllers
{
    public class HomeController : Controller
    {
        IRentService _rentService;
        public HomeController(IRentService serv)
        {
            _rentService = serv;
        }
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<CarViewModel>>(_rentService.GetCars()));
        }
               
        protected override void Dispose(bool disposing)
        {
            _rentService.Dispose();
            base.Dispose(disposing);
        }
    }
}