using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using NLog;

namespace CarRental.WEB.Areas.Manage.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class OrderManageController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly IRentService _rentService;

        public OrderManageController(IRentService serv)
        {
            _rentService = serv;
        }

        /// <summary>
        /// Shows the orders with status 'Unwatched'
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewOrders()
        {
            Logger.Debug("Request to Manage/NewOrders page. User: {0}", User.Identity.Name);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, OrderViewModel>();
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            return View(
                    mapper.Map<IEnumerable<OrderViewModel>>(
                        _rentService.GetOrders()
                            .Select(order => order)
                                .Where(order => order.OrderStatus == OrderDTO.Status.Unwatched).ToList()));
        }

        /// <summary>
        /// Shows the orders with statuses: 'Accepted', 'Paid', 'ReturnedWithDamage'
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CurrentOrders()
        {
            Logger.Debug("Request to Manage/CurrentOrders page. User: {0}", User.Identity.Name);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, OrderViewModel>();
                cfg.CreateMap<CarDTO, CarViewModel>();
            });
            var mapper = config.CreateMapper();
            return View(
                    mapper.Map<IEnumerable<OrderViewModel>>(
                        _rentService.GetOrders()
                            .Select(order => order)
                            .Where(
                                order =>
                                    order.OrderStatus == OrderDTO.Status.Accepted ||
                                    order.OrderStatus == OrderDTO.Status.Paid ||
                                    order.OrderStatus == OrderDTO.Status.ReturnedWithDamage).ToList()));
        }

        /// <summary>
        /// Sets order's status to 'Declined'
        /// </summary>
        /// <param name="order">Order with info from view</param>
        /// <returns>List of new orders</returns>
        [HttpPost]
        public ActionResult Decline(OrderViewModel order)
        {
            Logger.Debug("Attempt to decline order. Order id: {0}", order.Id);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Declined;
                orderDto.DeclineIssue = order.DeclineIssue;

                _rentService.UpdateOrder(orderDto);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                Logger.Info("Order #{6} declined by manager ({1}). Data (id: {6}, car: {0}, user: {2}, sum: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.ModelName, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return PartialView("Partials/_NewOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(or => or.OrderStatus == OrderDTO.Status.Unwatched).ToList()));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to decline order failed. Vlidation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}", ex.Property, ex.Message, order.Id);
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Sets order's status to 'Accepted'
        /// </summary>
        /// <param name="order">Order with info from view</param>
        /// <returns>List of new orders</returns>
        [HttpPost]
        public ActionResult Accept(OrderViewModel order)
        {
            Logger.Debug("Attempt to accept order. Order id: {0}", order.Id);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Accepted;

                _rentService.UpdateOrder(orderDto);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                Logger.Info("Order #{6} accepted by manager ({1}). Data (id: {6}, car: {0}, user: {2}, sum: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.ModelName, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return PartialView("Partials/_NewOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(or => or.OrderStatus == OrderDTO.Status.Unwatched).ToList()));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to accept order failed. Vlidation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}", ex.Property, ex.Message, order.Id);
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Change status from 'Declined' to 'Accepted'
        /// </summary>
        /// <param name="order">Order with info from view</param>
        /// <returns>List of current orders</returns>
        [HttpPost]
        public ActionResult DeclineAccepted(OrderViewModel order)
        {
            Logger.Debug("Attempt to decline accepted order. Order id: {0}", order.Id);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Declined;
                orderDto.DeclineIssue = order.DeclineIssue;

                _rentService.UpdateOrder(orderDto);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                Logger.Info("Order #{6} declined by manager ({1}). Data (id: {6}, car: {0}, user: {2}, sum: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.ModelName, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return PartialView("Partials/_CurrentOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(
                                    or =>
                                        or.OrderStatus == OrderDTO.Status.Accepted ||
                                        or.OrderStatus == OrderDTO.Status.Paid ||
                                        or.OrderStatus == OrderDTO.Status.ReturnedWithDamage).ToList()));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to decline accepted order failed. Vlidation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}", 
                    ex.Property, ex.Message, order.Id);
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Set status to 'Returned'
        /// </summary>
        /// <param name="order">Order with info from view</param>
        /// <returns>List of current orders</returns>
        [HttpPost]
        public ActionResult Return(OrderViewModel order)
        {
            Logger.Debug("Attempt to return order. Order id: {0}", order.Id);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Returned;

                _rentService.UpdateOrder(orderDto);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                Logger.Info("Order #{6} returned by manager ({1}). Data (id: {6}, car: {0}, user: {2}, sum: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.ModelName, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return PartialView("Partials/_CurrentOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(
                                    or =>
                                        or.OrderStatus == OrderDTO.Status.Accepted ||
                                        or.OrderStatus == OrderDTO.Status.Paid ||
                                        or.OrderStatus == OrderDTO.Status.ReturnedWithDamage).ToList()));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to return order failed. Vlidation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}", 
                    ex.Property, ex.Message, order.Id);
                return View("Error", ex);
            }
        }

        /// <summary>
        /// Set status to 'ReturnedWithDamage'
        /// </summary>
        /// <param name="order">Order with info from view</param>
        /// <returns>List of current orders</returns>
        [HttpPost]
        public ActionResult ReturnToRepair(OrderViewModel order)
        {
            Logger.Debug("Attempt to return order for repair. Order id: {0}", order.Id);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.ReturnedWithDamage;
                orderDto.RepairPrice = order.RepairPrice;

                _rentService.UpdateOrder(orderDto);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                Logger.Info("Order #{6} send for repairing by manager ({1}). Data (id: {6}, car: {0}, user: {2}, sum: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.ModelName, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return PartialView("Partials/_CurrentOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(
                                    or =>
                                        or.OrderStatus == OrderDTO.Status.Accepted ||
                                        or.OrderStatus == OrderDTO.Status.Paid ||
                                        or.OrderStatus == OrderDTO.Status.ReturnedWithDamage).ToList()));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to return order for repairing failed. Vlidation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}",
                    ex.Property, ex.Message, order.Id);
                return View("Error", ex);
            }
        }
    }
}