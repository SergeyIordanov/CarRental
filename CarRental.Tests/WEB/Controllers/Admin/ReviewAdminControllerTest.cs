using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.BLL.DTO;
using CarRental.BLL.Interfaces;
using CarRental.WEB.Areas.Admin.Controllers;
using CarRental.WEB.ViewModels;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Admin
{
    [TestFixture]
    public class ReviewAdminControllerTest
    {
        [Test]
        public void IndexReturnsReviewsList()
        {
            // Arrange
            var mock = new Mock<IRentService>();
            mock.Setup(a => a.GetReviews()).Returns(new List<ReviewDTO>());
            var controller = new ReviewAdminController(mock.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.TypeOf(typeof(List<ReviewViewModel>)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(null)]
        [TestCase(10000001)]
        public void DeleteReturnsPartialReviewsList(int? id)
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();

            var controller = new ReviewAdminController(mockRentService.Object);

            // Act
            var resultCreate = controller.Delete(id) as PartialViewResult;

            // Assert
            Assert.That(resultCreate, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(resultCreate.Model, Is.TypeOf(typeof(List<ReviewViewModel>)));
        }
    }
}
