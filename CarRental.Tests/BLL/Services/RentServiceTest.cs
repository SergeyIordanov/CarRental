using System;
using System.Collections.Generic;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Services;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.BLL.Services
{
    [TestFixture]
    public class RentServiceTest
    {
        #region Tests for creation

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase(null, "ok", "ok", 1)]
        [TestCase("", "ok", "ok", 1)]
        [TestCase("ok", null, "ok", 1)]
        [TestCase("ok", "", "ok", 1)]
        [TestCase("ok", "ok", "", 1)]
        [TestCase("ok", "ok", null, 1)]
        [TestCase("ok", "ok", "ok", -1)]
        [TestCase("ok", "ok", "ok", -1000)]
        public void CreateCarValidationTest(string modelName, string brand, string carClass, int? seats)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.Create(It.IsAny<Car>()));

            //Act
            var service = new RentService(mockUow.Object);
                            
            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.CreateCar(new CarDTO {ModelName = modelName, Brand = brand, Class = carClass, Seats = seats}));
        }

        [Test]
        [TestCase(null, null, null, null, null, null, null, null)]
        [TestCase("ok", "ok", "ok", "+380955555555", "ok", "10.10.2016", "10.10.2016", null)]
        [TestCase("ok", "ok", "ok", "+380955555555", "ok", "10.10.2016", null, 1)]
        [TestCase("ok", "ok", "ok", "+380955555555", "ok", null, "10.10.2016", 1)]
        [TestCase("ok", "ok", "ok", "+380955555555", null, "10.10.2016", "10.10.2016", 1)]
        [TestCase("ok", "ok", "ok", "+380955555555", "", "10.10.2016", "10.10.2016", 1)]
        [TestCase("ok", "ok", "ok", "123213", "ok", "10.10.2016", "10.10.2016", 1)]
        [TestCase("ok", "ok", "ok", null, "ok", "10.10.2016", "10.10.2016", 1)]
        [TestCase("ok", "ok", "", "+380955555555", "ok", "10.10.2016", "10.10.2016", 1)]
        [TestCase("ok", "ok", null, "+380955555555", "ok", "10.10.2016", "10.10.2016", 1)]
        [TestCase("", "ok", "ok", "+380955555555", "ok", "10.10.2016", "10.10.2016", 1)]
        [TestCase(null, "ok", "ok", "+380955555555", "ok", "10.10.2016", "10.10.2016", 1)]
        public void CreateOrderValidationTest(string userId, string firstName, string lastName, string phoneNumber, string pickUpAdress, DateTime fromDate, DateTime toDate, int? carId)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.Create(It.IsAny<Order>()));

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.CreateOrder(new OrderDTO
                    {
                        UserId = userId,
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        FromDate = fromDate,
                        ToDate = toDate,
                        PickUpAddress = pickUpAdress
                    }, carId));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("ok", null)]
        [TestCase(null, "10.10.2016")]
        public void CreateReviewValidationTest(string text, DateTime publishDate)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Reviews.Create(It.IsAny<Review>()));

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.CreateReview(new ReviewDTO { Text = text, PublishDate = publishDate}));
        }

        #endregion

        #region Tests for updating

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase(null, "ok", "ok", 1)]
        [TestCase("", "ok", "ok", 1)]
        [TestCase("ok", null, "ok", 1)]
        [TestCase("ok", "", "ok", 1)]
        [TestCase("ok", "ok", "", 1)]
        [TestCase("ok", "ok", null, 1)]
        [TestCase("ok", "ok", "ok", -1)]
        [TestCase("ok", "ok", "ok", -1000)]
        public void UpdateCarValidationTest(string modelName, string brand, string carClass, int? seats)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.Update(It.IsAny<Car>()));

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.UpdateCar(new CarDTO { ModelName = modelName, Brand = brand, Class = carClass, Seats = seats }));
        }

        [Test]
        [TestCase(null, null, null, null, null, null, null)]
        [TestCase("ok", "ok", "ok", "+380955555555", "ok", "10.10.2016", "10.10.2016")]
        [TestCase("ok", "ok", "ok", "+380955555555", "ok", "10.10.2016", null)]
        [TestCase("ok", "ok", "ok", "+380955555555", "ok", null, "10.10.2016")]
        [TestCase("ok", "ok", "ok", "+380955555555", null, "10.10.2016", "10.10.2016")]
        [TestCase("ok", "ok", "ok", "+380955555555", "", "10.10.2016", "10.10.2016")]
        [TestCase("ok", "ok", "ok", "123213", "ok", "10.10.2016", "10.10.2016")]
        [TestCase("ok", "ok", "ok", null, "ok", "10.10.2016", "10.10.2016")]
        [TestCase("ok", "ok", "", "+380955555555", "ok", "10.10.2016", "10.10.2016")]
        [TestCase("ok", "ok", null, "+380955555555", "ok", "10.10.2016", "10.10.2016")]
        [TestCase("", "ok", "ok", "+380955555555", "ok", "10.10.2016", "10.10.2016")]
        [TestCase(null, "ok", "ok", "+380955555555", "ok", "10.10.2016", "10.10.2016")]
        public void UpdateOrderValidationTest(string userId, string firstName, string lastName, string phoneNumber, string pickUpAdress, DateTime fromDate, DateTime toDate)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.Update(It.IsAny<Order>()));

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.UpdateOrder(new OrderDTO
                    {
                        UserId = userId,
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        FromDate = fromDate,
                        ToDate = toDate,
                        PickUpAddress = pickUpAdress
                    }));
        }

        #endregion

        #region Tests for deleting

        [Test]
        [TestCase(null)]
        [TestCase(-1)]
        public void DeleteCarValidationTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.Get(-1)).Returns((Car) null);

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.DeleteCar(id));
        }

        [Test]
        [TestCase(null)]
        [TestCase(-1)]
        public void DeleteOrderValidationTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.Get(-1)).Returns((Order)null);

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.DeleteOrder(id));
        }

        [Test]
        [TestCase(null)]
        [TestCase(-1)]
        public void DeleteReviewValidationTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Reviews.Get(-1)).Returns((Review)null);

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.DeleteReview(id));
        }

        #endregion

        #region Tests for getting

        [Test]
        [TestCase(1)]
        [TestCase(-10)]
        [TestCase(0)]
        public void GetCarTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.Get(It.IsAny<int>())).Returns(new Car());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetCar(id);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(CarDTO)));
                    
        }

        [Test]
        [TestCase(null)]
        [TestCase(1)]
        public void GetCarValidationTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.Get(It.IsAny<int>())).Returns((Car)null);

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.GetCar(id));
        }

        [Test]
        public void GetCarsTest()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.GetAll()).Returns(new List<Car>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetCars();

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<CarDTO>)));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("some text")]
        [TestCase("text")]
        public void SearchCarsByStringTest(string searchString)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.GetAll()).Returns(new List<Car>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetCars(searchString);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<CarDTO>)));
        }

        [Test]
        public void SearchCarsByFilterTest()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Cars.GetAll()).Returns(new List<Car>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetCars(new FilterDTO());

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<CarDTO>)));
        }

        [Test]
        public void GetReviewsTest()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Reviews.GetAll()).Returns(new List<Review>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetReviews();

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<ReviewDTO>)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(-10)]
        [TestCase(0)]
        public void GetOrderTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.Get(It.IsAny<int>())).Returns(new Order());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetOrder(id);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(OrderDTO)));
        }

        [Test]
        [TestCase(null)]
        [TestCase(-10)]
        public void GetOrderValidationTest(int? id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.Get(It.IsAny<int>())).Returns((Order)null);

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.GetOrder(id));
        }

        [Test]
        public void GetOrdersTest()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.GetAll()).Returns(new List<Order>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetOrders();

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<OrderDTO>)));
        }

        [Test]
        [TestCase("id")]
        public void GetOrderByUserIdTest(string userId)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.GetAll()).Returns(new List<Order>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetOrders(userId);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<OrderDTO>)));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void GetOrderByUserIdValidationTest(string id)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.Get(It.IsAny<int>())).Returns((Order)null);

            //Act
            var service = new RentService(mockUow.Object);

            //Assert
            Assert.Throws<ValidationException>(
                () =>
                    service.GetOrders(id));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(null, "")]
        [TestCase(null, "ok")]
        [TestCase("ok", "")]
        [TestCase("ok", "ok")]
        public void SearchOrdersByCarAndUserTest(string searchCar, string searchUser)
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.GetAll()).Returns(new List<Order>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetOrders(searchCar, searchUser);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<OrderDTO>)));
        }

        [Test]
        public void GetCurrentLogReturnsString()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(a => a.Orders.GetAll()).Returns(new List<Order>());

            //Act
            var service = new RentService(mockUow.Object);
            var result = service.GetCurrentLog("somePath");

            //Assert
            Assert.That(result, Is.TypeOf(typeof(string)));
        }

        #endregion

    }
}
