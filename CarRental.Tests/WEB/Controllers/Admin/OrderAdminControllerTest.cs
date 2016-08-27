using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Areas.Admin.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Admin
{
    [TestFixture]
    public class OrderAdminControllerTest
    {
        [Test]
        public void IndexViewReturnsOrdersLsit()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetOrders()).Returns(new List<OrderDTO>());
            var controller = new OrderAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(List<OrderViewModel>)));
        }

        [Test]
        public void SearchPostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetOrders()).Returns(new List<OrderDTO>());

            var controller = new OrderAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult result = controller.Search("test", "test") as PartialViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(10000001)]
        public void DeleteReturnsPartialOrdersList(int? id)
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();

            var controller = new OrderAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            var resultCreate = controller.Delete(id) as PartialViewResult;

            // Assert
            Assert.That(resultCreate, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultCreate.Model, Is.TypeOf(typeof(List<OrderViewModel>)));
        }
    }
}
