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

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}
