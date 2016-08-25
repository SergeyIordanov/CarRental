using CarRental.WEB.Areas.Admin.Controllers;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Admin
{
    [TestFixture]
    public class AdminControllerTest
    {
        [Test]
        public void IndexViewNotNull()
        {
            // Arrange

            AdminController controller = new AdminController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}
