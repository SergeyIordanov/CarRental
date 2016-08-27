using System.Collections.Generic;
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
    public class OrderAdminController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            Logger.Debug("Request to Admin/Orders page. User: {0}", User.Identity.Name);
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
                Logger.Warn("Request to Admin/Orders page failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
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
            Logger.Debug("Admin search order request. Data ( car: '{0}', user: '{1}'", searchCar, searchUser);
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
            Logger.Debug("Attempt to delete an order. Order id: {0}", id);
            try
            {
                _rentService.DeleteOrder(id);
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Order deleting failed. Validation error (Property: {0}, Message: {1})", ex.Property, ex.Message);
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, OrderViewModel>();
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();

            Logger.Info("Order #{0} was deleted by {1}.", id, User.Identity.Name);

            return PartialView("Partials/_OrdersList", mapper.Map<IEnumerable<OrderViewModel>>(_rentService.GetOrders()));
        }
    }
}