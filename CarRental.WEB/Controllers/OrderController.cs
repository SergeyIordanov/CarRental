using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;
using Microsoft.AspNet.Identity;

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
        [ValidateAntiForgeryToken]
        public ActionResult Index(OrderViewModel order, int carId)
        {
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
                orderDto.Car = _rentService.GetCar(carId);
                orderDto.TotalPrice = (orderDto.ToDate.ToUniversalTime() - orderDto.FromDate.ToUniversalTime()).Days *
                                      orderDto.Car.PriceForDay;
                if (orderDto.WithDriver)
                    orderDto.TotalPrice += 20 *
                                           (orderDto.ToDate.ToUniversalTime() - orderDto.FromDate.ToUniversalTime()).Days;

                _rentService.CreateOrder(orderDto);

                config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderDTO, OrderViewModel>();
                    cfg.CreateMap<CarDTO, CarViewModel>();
                });
                mapper = config.CreateMapper();                

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
                return View(order);
            }           
        }

        [HttpGet]
        public ActionResult UserOrders()
        {
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
                return View("Error", ex);
            }
        }

        [HttpGet]
        public ActionResult Bill(int? orderId)
        {
            if (orderId == null)
            {
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
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bill(OrderViewModel order)
        {
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Paid;

                _rentService.UpdateOrder(orderDto);

                return RedirectToAction("UserOrders");
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }

        [HttpGet]
        public ActionResult RepairBill(int? orderId)
        {
            if (orderId == null)
            {
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
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RepairBill(OrderViewModel order)
        {
            try
            {
                var orderDto = _rentService.GetOrder(order.Id);

                orderDto.OrderStatus = OrderDTO.Status.Returned;

                _rentService.UpdateOrder(orderDto);

                return RedirectToAction("UserOrders");
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }
    }
}