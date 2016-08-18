using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.WEB.ViewModels;

namespace CarRental.WEB.Areas.Manage.Controllers
{
    public class OrderManageController : Controller
    {
        readonly IRentService _rentService;
        public OrderManageController(IRentService serv)
        {
            _rentService = serv;
        }

        [HttpGet]
        public ActionResult NewOrders()
        {
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

        [HttpGet]
        public ActionResult CurrentOrders()
        {
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

        [HttpPost]
        public ActionResult Decline(OrderViewModel order)
        {
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
                return PartialView("Partials/_NewOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(or => or.OrderStatus == OrderDTO.Status.Unwatched).ToList()));
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public ActionResult Accept(OrderViewModel order)
        {
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
                return PartialView("Partials/_NewOrdersList",
                        mapper.Map<IEnumerable<OrderViewModel>>(
                            _rentService.GetOrders()
                                .Select(or => or)
                                .Where(or => or.OrderStatus == OrderDTO.Status.Unwatched).ToList()));
            }
            catch (ValidationException ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        public ActionResult DeclineAccepted(OrderViewModel order)
        {
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
                return View("Error", ex);
            }
        }

        [HttpPost]
        public ActionResult Return(OrderViewModel order)
        {
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
                return View("Error", ex);
            }
        }

        [HttpPost]
        public ActionResult ReturnToRepair(OrderViewModel order)
        {
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
                return View("Error", ex);
            }
        }
    }
}