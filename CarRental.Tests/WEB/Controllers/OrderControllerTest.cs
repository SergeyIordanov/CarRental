using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers
{
    [TestFixture]
    public class OrderControllerTest
    {
        [Test]
        public void IndexWithNullIdReturnsHttpNotFound()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetCar(1)).Returns(new CarDTO());
            OrderController controller = new OrderController(mock.Object);            

            // Act
            var result = controller.Index(null);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(HttpNotFoundResult)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100000001)]
        public void IndexWithWrongIdReturnsErrorView(int id)
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetCar(id)).Throws(new ValidationException("test", "test"));
            OrderController controller = new OrderController(mock.Object);

            // Act
            ViewResult result = controller.Index(id) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100000001)]
        public void IndexWithCorrectIdReturnsOrderViewModel(int id)
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetCar(id)).Returns(new CarDTO());
            OrderController controller = new OrderController(mock.Object);

            // Act
            ViewResult result = controller.Index(id) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(OrderViewModel)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100000001)]
        public void IndexPostReturnsOrderViewModel(int id)
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetCar(id)).Returns(new CarDTO());
            OrderController controller = new OrderController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller);

            // Act
            ViewResult result = controller.Index(new OrderViewModel(), id) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(OrderViewModel)));
        }

        [Test]
        public void UserOrdersReturnsOrderViewModelList()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrders(null)).Returns(new List<OrderDTO>());
            OrderController controller = new OrderController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller);

            // Act
            ViewResult result = controller.UserOrders() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(List<OrderViewModel>)));
        }

        [Test]
        public void UserOrdersReturnsErrorViewOnValidationException()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrders(null)).Throws(new ValidationException("test", "test"));
            OrderController controller = new OrderController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller);

            // Act
            ViewResult result = controller.UserOrders() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        [Test]
        public void BillWithNullIdReturnsHttpNotFound()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrder(1)).Returns(new OrderDTO());
            OrderController controller = new OrderController(mock.Object);

            // Act
            var result = controller.Bill(orderId: null);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(HttpNotFoundResult)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100000001)]
        public void BillWithWrongIdReturnsErrorView(int id)
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrder(id)).Throws(new ValidationException("test", "test"));
            OrderController controller = new OrderController(mock.Object);

            // Act
            ViewResult result = controller.Bill(id) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100000001)]
        public void BillWithCorrectIdReturnsOrderViewModel(int id)
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetOrder(id)).Returns(new OrderDTO());
            OrderController controller = new OrderController(mock.Object);

            // Act
            ViewResult result = controller.Bill(id) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(OrderViewModel)));
        }

    }
}
