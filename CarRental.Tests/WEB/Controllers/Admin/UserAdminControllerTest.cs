using System.Collections.Generic;
using System.Web.Mvc;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Areas.Admin.Controllers;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers.Admin
{
    [TestFixture]
    public class UserAdminControllerTest
    {
        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase(null, "")]
        [TestCase("", "")]
        public void SetRoleReturnsHttpNotFoundOnWrongArguments(string id, string role)
        {
            // Arrange
            var controller = new UserAdminController();
            controller.ControllerContext = new FakeControllerContext(controller);

            // Act
            var result = controller.SetRole(id, role);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(HttpNotFoundResult)));
        }
    }
}
