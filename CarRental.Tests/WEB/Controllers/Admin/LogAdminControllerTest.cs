using System.Web.Mvc;
using CarRental.BLL.Interfaces;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Areas.Admin.Controllers;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Admin
{
    [TestFixture]
    public class LogAdminControllerTest
    {
        [Test]
        public void IndexViewNotNull()
        {
            // Arrange
            var mockRentService = new Mock<IRentService>();
            mockRentService.Setup(a => a.GetCurrentLog(It.IsAny<string>())).Returns("currentLog");
            var controller = new LogAdminController(mockRentService.Object);
            controller.ControllerContext = new FakeControllerContext(controller);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(result.Model, Is.EqualTo("currentLog"));
        }
    }
}
