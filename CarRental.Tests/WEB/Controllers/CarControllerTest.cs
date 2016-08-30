using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers
{
    [TestFixture]
    public class CarControllerTest
    {
        [Test]
        public void IndexGetViewNotNull()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetCars()).Returns(new List<CarDTO>());
            var controller = new CarController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void IndexGetViewModelNotNull()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetCars()).Returns(new List<CarDTO>());
            var controller = new CarController(mock.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void SearchPostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars()).Returns(new List<CarDTO>());

            var controller = new CarController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult result = controller.Search("test") as PartialViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void ShowFiltersReturnsFilterViewModel()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars()).Returns(new List<CarDTO>());

            var controller = new CarController(mockRentService.Object);

            // Act
            PartialViewResult result = controller.ShowFilters() as PartialViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(FilterViewModel)));
        }

        [Test]
        public void FilterPostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars(new FilterDTO())).Returns(new List<CarDTO>());

            var controller = new CarController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult result = controller.Filter(new FilterViewModel()) as PartialViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void SortByNamePostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars()).Returns(new List<CarDTO>());

            var controller = new CarController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult resultAsc = controller.SortByName() as PartialViewResult;
            PartialViewResult resultDesc = controller.SortByName(true) as PartialViewResult;
            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultAsc.Model, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultDesc.Model, Is.Not.Null);
        }

        [Test]
        public void SortByPricePostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCars()).Returns(new List<CarDTO>());

            var controller = new CarController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            PartialViewResult resultAsc = controller.SortByPrice() as PartialViewResult;
            PartialViewResult resultDesc = controller.SortByPrice(true) as PartialViewResult;
            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultAsc.Model, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultDesc.Model, Is.Not.Null);
        }
    }
}
