using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.WEB.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;
using CarRental.Tests.WEB.Fakes;

namespace CarRental.Tests.WEB.Controllers
{
    [TestFixture]
    public class ReviewControllerTest
    {
        [Test]
        public void IndexGetViewNotNull()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetReviews()).Returns(new List<ReviewDTO>());
            ReviewController controller = new ReviewController(mock.Object);

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
            mock.Setup(a => a.GetReviews()).Returns(new List<ReviewDTO>());
            ReviewController controller = new ReviewController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        public void IndexPostViewModelNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetReviews()).Returns(new List<ReviewDTO>());           

            var controller = new ReviewController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller);

            // Act
            PartialViewResult result = controller.Index(new ReviewViewModel()) as PartialViewResult;

            // Assert
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.Not.Null);
        }
    }
}
