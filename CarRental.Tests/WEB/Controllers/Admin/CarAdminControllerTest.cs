using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Areas.Admin.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Admin
{
    [TestFixture]
    public class CarAdminControllerTest
    {
        [Test]
        public void IndexViewReturnsCarsLsit()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars()).Returns(new List<CarDTO>());
            var controller = new CarAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(List<CarViewModel>)));
        }

        [Test]
        public void SearchPostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars()).Returns(new List<CarDTO>());

            var controller = new CarAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult result = controller.Search("test") as PartialViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void CreateAndEditRedirectToIndexOnSuccess()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCar(It.IsAny<int>())).Returns(new CarDTO());

            var controller = new CarAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            var resultCreate = controller.Create(new CarViewModel(), null);
            var resultEdit = controller.Edit(new CarViewModel(), null);

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultCreate, Is.TypeOf(typeof(RedirectToRouteResult)));
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultEdit, Is.TypeOf(typeof(RedirectToRouteResult)));
        }

        [Test]
        public void CreateAndEditReturnTheirViewsOnValidationException()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCar(0)).Throws(new ValidationException("test", "test"));
            mockRentService.Setup(a => a.UpdateCar(It.IsAny<CarDTO>())).Throws(new ValidationException("test", "test"));
            mockRentService.Setup(a => a.CreateCar(It.IsAny<CarDTO>())).Throws(new ValidationException("test", "test"));

            var controller = new CarAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            var resultCreate = controller.Create(new CarViewModel(), null) as ViewResult;
            var resultEdit = controller.Edit(new CarViewModel(), null) as ViewResult;

            // Assert
            Assert.That(resultCreate, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultCreate.Model, Is.TypeOf(typeof(CarViewModel)));

            Assert.That(resultEdit, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultEdit.Model, Is.TypeOf(typeof(CarViewModel)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(10000001)]
        public void DeleteReturnsPartialCarsList(int? id)
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCar(0)).Throws(new ValidationException("test", "test"));
            mockRentService.Setup(a => a.UpdateCar(It.IsAny<CarDTO>())).Throws(new ValidationException("test", "test"));
            mockRentService.Setup(a => a.CreateCar(It.IsAny<CarDTO>())).Throws(new ValidationException("test", "test"));

            var controller = new CarAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            var resultCreate = controller.Delete(id) as PartialViewResult;

            // Assert
            Assert.That(resultCreate, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultCreate.Model, Is.TypeOf(typeof(List<CarViewModel>)));
        }
    }
}
