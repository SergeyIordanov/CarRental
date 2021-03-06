﻿using System.Web.Mvc;
using CarRental.Tests.WEB.Fakes;
using CarRental.WEB.Controllers;
using NUnit.Framework;

namespace CarRental.Tests.WEB.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void IndexViewNotNull()
        {
            // Arrange
            var controller = new HomeController();
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ContactsViewNotNull()
        {
            // Arrange
            var controller = new HomeController();
            controller.ControllerContext = new FakeControllerContext(controller, new FormCollection());

            // Act
            ViewResult result = controller.Contacts() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}
