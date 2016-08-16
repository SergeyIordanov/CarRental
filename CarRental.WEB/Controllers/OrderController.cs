using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;

namespace CarRental.WEB.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        readonly IRentService _rentService;
        public OrderController(IRentService serv)
        {
            _rentService = serv;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? carId)
        {
            if (carId == null)
            {
                return HttpNotFound();
            }
            try
            {
                var orderView = new OrderViewModel();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();
                orderView.Car = mapper.Map<CarViewModel>(_rentService.GetCar(carId));
                return View(orderView);
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public ActionResult Index(OrderViewModel order, int carId)
        {
            return View();
        }
    }
}