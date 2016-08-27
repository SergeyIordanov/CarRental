using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Areas.Manage.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Manage
{
    [TestFixture]
    public class OrderManageControllerTest
    {
        [Test]
        public void NewOrdersReturnsOrdersList()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrders()).Returns(new List<OrderDTO>());
            var controller = new OrderManageController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.NewOrders() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(List<OrderViewModel>)));
        }

        [Test]
        public void CurrentOrdersReturnsOrdersList()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrders()).Returns(new List<OrderDTO>());
            var controller = new OrderManageController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.CurrentOrders() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(List<OrderViewModel>)));
        }

        [Test]
        public void AllActionsWithOrderStatusReturnsOrdersList()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrder(It.IsAny<int>())).Returns(new OrderDTO
            {
                Car = new CarDTO { Brand = "test", ModelName = "test" },
                FirstName = "test",
                LastName = "test",
                ToDate = DateTime.Now.AddDays(2),
                FromDate = DateTime.Now,
                Id = 1,
                TotalPrice = 100
            });
            mock.Setup(a => a.GetOrders()).Returns(new List<OrderDTO>());

            var controller = new OrderManageController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult resultDecline = controller.Decline(new OrderViewModel
                {
                    Car = new CarViewModel { Brand = "test", ModelName = "test" },
                    FirstName = "test",
                    LastName = "test",
                    ToDate = DateTime.Now.AddDays(2),
                    FromDate = DateTime.Now,
                    Id = 1,
                    TotalPrice = 100
                }) as PartialViewResult;
            PartialViewResult resultAccept = controller.Accept(new OrderViewModel()) as PartialViewResult;
            PartialViewResult resultReturn = controller.Return(new OrderViewModel()) as PartialViewResult;
            PartialViewResult resultReturnToRepair = controller.ReturnToRepair(new OrderViewModel()) as PartialViewResult;
            PartialViewResult resultDeclineAccepted = controller.DeclineAccepted(new OrderViewModel()) as PartialViewResult;

            // Assert
            Assert.That(resultDecline, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultDecline.Model, Is.TypeOf(typeof(List<OrderViewModel>)));

            Assert.That(resultAccept, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultAccept.Model, Is.TypeOf(typeof(List<OrderViewModel>)));

            Assert.That(resultReturn, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultReturn.Model, Is.TypeOf(typeof(List<OrderViewModel>)));

            Assert.That(resultReturnToRepair, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultReturnToRepair.Model, Is.TypeOf(typeof(List<OrderViewModel>)));

            Assert.That(resultDeclineAccepted, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultDeclineAccepted.Model, Is.TypeOf(typeof(List<OrderViewModel>)));
        }

        [Test]
        public void AllActionsWithOrderStatusReturnsErrorViewOnValidationException()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrder(It.IsAny<int>())).Throws(new ValidationException("test", "test"));
            mock.Setup(a => a.GetOrders()).Returns(new List<OrderDTO>());
            
            var controller = new OrderManageController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult resultDecline = controller.Decline(new OrderViewModel()) as ViewResult;
            ViewResult resultAccept = controller.Accept(new OrderViewModel()) as ViewResult;
            ViewResult resultReturn = controller.Return(new OrderViewModel()) as ViewResult;
            ViewResult resultReturnToRepair = controller.ReturnToRepair(new OrderViewModel()) as ViewResult;
            ViewResult resultDeclineAccepted = controller.DeclineAccepted(new OrderViewModel()) as ViewResult;

            // Assert
            Assert.That(resultDecline, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultDecline.ViewName, Is.EqualTo("Error"));

            Assert.That(resultAccept, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultAccept.ViewName, Is.EqualTo("Error"));

            Assert.That(resultReturn, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultReturn.ViewName, Is.EqualTo("Error"));

            Assert.That(resultReturnToRepair, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultReturnToRepair.ViewName, Is.EqualTo("Error"));

            Assert.That(resultDeclineAccepted, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultDeclineAccepted.ViewName, Is.EqualTo("Error"));
        }
    }
}
