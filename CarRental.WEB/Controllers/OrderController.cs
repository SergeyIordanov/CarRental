using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using Microsoft.AspNet.Identity;
using NLog;

namespace CarRental.WEB.Controllers
{
    [Authorize(Roles = "user, manager, admin")]
    public class OrderController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly IRentService _rentService;

        public OrderController(IRentService serv)
        {
            _rentService = serv;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? carId)
        {
            Logger.Debug("Request to order page. Car id:{0}  User: {1}", carId, string.IsNullOrEmpty(User.Identity.Name) ? "Anonymous" : User.Identity.Name);
            if (carId == null)
            {
                Logger.Debug("Wrong request. Car id is null. 404 returned");
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
                Logger.Debug("Wrong request. Car id was not found. Error page returned");
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(OrderViewModel order, int carId)
        {
            Logger.Debug("Order creation attempt. User: {0}", User.Identity.Name);
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderViewModel, OrderDTO>();
                    cfg.CreateMap<CarViewModel, CarDTO>();
                });
                var mapper = config.CreateMapper();
                var orderDto = mapper.Map<OrderDTO>(order);

                orderDto.UserId = User.Identity.GetUserId();
                orderDto.OrderStatus = OrderDTO.Status.Unwatched;                             

                _rentService.CreateOrder(orderDto, carId);

                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                mapper = config.CreateMapper();

                Logger.Info("New order is created. Data (id: {6}, car: {0}, user: {1}, user_name: {2} sum: {3}, from: {4}, to: {5})", 
                    orderDto.Car.Brand + " " + orderDto.Car.Class, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return View("OrderSuccess", mapper.Map<OrderViewModel>(orderDto));
            }
            catch (ValidationException ex)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();
                order.Car = mapper.Map<CarViewModel>(_rentService.GetCar(carId));
                ModelState.AddModelError(ex.Property, ex.Message);

                Logger.Debug("Order creation failed. Validation error");

                return View(order);
            }           
        }

        [HttpGet]
        public ActionResult UserOrders()
        {
            Logger.Debug("Request to user orders page. User: {0}", User.Identity.Name);
            try
            {
                var ordersDto = _rentService.GetOrders(User.Identity.GetUserId());

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                return View(mapper.Map<IEnumerable<OrderViewModel>>(ordersDto));
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Request to user orders page failed. Validation error (Property: {0}, Message: {1}). Error page returned. User: {2}", 
                    ex.Property, ex.Message,  User.Identity.Name);

                return View("Error", ex);
            }
        }

        [HttpGet]
        public ActionResult Bill(int? orderId)
        {
            Logger.Debug("Request to Bill page. Order id: {0}, User: {1}", orderId, User.Identity.Name);
            if (orderId == null)
            {
                Logger.Debug("Wrong request. Order id is null. 404 returned");
                return HttpNotFound();
            }
            try
            {
                var orderDto = _rentService.GetOrder(orderId);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                return View(mapper.Map<OrderViewModel>(orderDto));
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Wrong request. Order was not found. Error page returned");
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bill(OrderViewModel order)
        {
            Logger.Debug("Attempt to pay the bill. Order id: {0}, User: {1}", order.Id, User.Identity.Name);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Paid;

                _rentService.UpdateOrder(orderDto);

                Logger.Info("The order #{6} is paid. Data ( id: {6}, car: {0}, user: {1}, user_name: {2} sum: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.Class, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.TotalPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);
                return RedirectToAction("UserOrders");
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to pay the bill failed. Validation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}, User: {3}", 
                    ex.Property, ex.Message, order.Id, User.Identity.Name);
                return View("Error", ex);
            }
        }

        [HttpGet]
        public ActionResult RepairBill(int? orderId)
        {
            Logger.Debug("Request to RepairBill page. Order id: {0}, User: {1}", orderId, User.Identity.Name);
            if (orderId == null)
            {
                Logger.Debug("Wrong request. Order id is null. 404 returned");
                return HttpNotFound();
            }
            try
            {
                var orderDto = _rentService.GetOrder(orderId);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                var mapper = config.CreateMapper();

                return View(mapper.Map<OrderViewModel>(orderDto));
            }
            catch (ValidationException ex)
            {
                Logger.Debug("Wrong request. Order was not found. Error page returned");
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RepairBill(OrderViewModel order)
        {
            Logger.Debug("Attempt to pay the repair bill. Order id: {0}, User: {1}", order.Id, User.Identity.Name);
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Returned;

                _rentService.UpdateOrder(orderDto);

                Logger.Info("The repair bill for order #{6} is paid. Data ( id: {6}, car: {0}, user: {1}, user_name: {2} repair_price: {3}, from: {4}, to: {5})",
                    orderDto.Car.Brand + " " + orderDto.Car.Class, User.Identity.Name, orderDto.FirstName + " " + orderDto.LastName,
                    orderDto.RepairPrice, orderDto.FromDate.ToShortDateString(), orderDto.ToDate.ToShortDateString(), orderDto.Id);

                return RedirectToAction("UserOrders");
            }
            catch (ValidationException ex)
            {
                Logger.Warn("Attempt to pay the bill failed. Validation error (Property: {0}, Message: {1}). Error page returned. Order id: {2}, User: {3}",
                    ex.Property, ex.Message, order.Id, User.Identity.Name);
                return View("Error", ex);
            }
        }
    }
}