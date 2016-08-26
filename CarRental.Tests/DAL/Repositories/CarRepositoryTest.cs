using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Repositories;
using CarRental.Entities.General;
using Moq;
using NUnit.Framework;

namespace CarRental.Tests.DAL.Repositories
{
    [TestFixture]
    public class CarRepositoryTest
    {
        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CarsData))]
        public void GetAllReturnsCarsList(IQueryable<Car> data)
        {
            //Arrange                       
            var mockSet = new Mock<DbSet<Car>>();
            mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Cars).Returns(mockSet.Object);

            //Act
            var repository = new CarRepository(mock.Object);
            var result = repository.GetAll();

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<Car>)));
        }

        [Test]
        public void GetReturnsCarOnCorrectId()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Car>>();

            mockSet.Setup(a => a.Find(It.IsAny<int>())).Returns(new Car());

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Cars).Returns(mockSet.Object);

            //Act
            var repository = new CarRepository(mock.Object);
            var result = repository.Get(1);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(Car)));
        }

        [Test]
        public void CreateCallsAdd()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Car>>();

            mockSet.Setup(a => a.Add(It.IsAny<Car>())).Verifiable();

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Cars).Returns(mockSet.Object);

            //Act
            var repository = new CarRepository(mock.Object);
            repository.Create(new Car());

            //Assert
            Mock.Verify(mockSet);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.CarsData))]
        public void FindReturnsCorrectCarsList(IQueryable<Car> data)
        {
            //Arrange                       
            var mockSet = new Mock<DbSet<Car>>();
            mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Cars).Returns(mockSet.Object);

            //Act
            var repository = new CarRepository(mock.Object);
            var result = repository.Find(car => car.Brand.Equals("Renault"));

            //Assert
            Assert.That(result, Has.Count.EqualTo(2).And.TypeOf(typeof(List<Car>)));
        }

        [Test]
        public void DeleteCallRemoveOnCorrectId()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Car>>();

            mockSet.Setup(a => a.Remove(It.IsAny<Car>())).Verifiable();
            mockSet.Setup(a => a.Find(It.IsAny<int>())).Returns(new Car());

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Cars).Returns(mockSet.Object);

            //Act
            var repository = new CarRepository(mock.Object);
            repository.Delete(1);

            //Assert
            Mock.Verify(mockSet);
        }
    }
}
