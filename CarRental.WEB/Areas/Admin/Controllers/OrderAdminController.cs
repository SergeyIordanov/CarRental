using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderAdminController : Controller
    {
        readonly IRentService _rentService;

        public OrderAdminController(IRentService serv)
        {
            _rentService = serv;
        }

        /// <summary>
        /// Shows the orders list
        /// </summary>
        /// <returns>View with the orders list</returns>
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                return View(mapper.Map<IEnumerable<OrderViewModel>>(_rentService.GetOrders()));
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Seraching for orders by car brand & model and/or user name
        /// </summary>
        /// <param name="searchCar">Car brand and/or model</param>
        /// <param name="searchUser">User name</param>
        /// <returns>List of orders</returns>
        [HttpPost]
        public ActionResult Search(string searchCar, string searchUser)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, OrderViewModel>();
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            var orders = mapper.Map<IEnumerable<OrderViewModel>>(_rentService.GetOrders(searchCar, searchUser));
            return PartialView("Partials/_OrdersList", orders);
        }

        /// <summary>
        /// Deleting an order
        /// </summary>
        /// <param name="id">Order id to delete</param>
        /// <returns>List of orders</returns>
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            try
            {
                _rentService.DeleteOrder(id);
            }
            catch (ValidationException) { }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, OrderViewModel>();
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();

            return PartialView("Partials/_OrdersList", mapper.Map<IEnumerable<OrderViewModel>>(_rentService.GetOrders()));
        }
    }
}