using System.Web.Mvc;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Areas.Manage.Controllers;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Manage
{
    [TestFixture]
    public class ManageControllerTest
    {
        [Test]
        public void IndexViewNotNull()
        {
            // Arrange

            ManageController controller = new ManageController();
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}
