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
    public class OrderRepositoryTest
    {

        [Test]
        public void GetReturnsOrderOnCorrectId()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Order>>();

            mockSet.Setup(a => a.Find(It.IsAny<int>())).Returns(new Order());

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Orders).Returns(mockSet.Object);

            //Act
            var repository = new OrderRepository(mock.Object);
            var result = repository.Get(1);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(Order)));
        }

        [Test]
        public void CreateCallsAdd()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Order>>();

            mockSet.Setup(a => a.Add(It.IsAny<Order>())).Verifiable();

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Orders).Returns(mockSet.Object);

            //Act
            var repository = new OrderRepository(mock.Object);
            repository.Create(new Order());

            //Assert
            Mock.Verify(mockSet);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.OrdersData))]
        public void FindReturnsCorrectOrdersList(IQueryable<Order> data)
        {
            //Arrange                       
            var mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Orders).Returns(mockSet.Object);

            //Act
            var repository = new OrderRepository(mock.Object);
            var result = repository.Find(order => order.FirstName.Equals("test1"));

            //Assert
            Assert.That(result, Has.Count.EqualTo(1).And.TypeOf(typeof(List<Order>)));
        }

        [Test]
        public void DeleteCallRemoveOnCorrectId()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Order>>();

            mockSet.Setup(a => a.Remove(It.IsAny<Order>())).Verifiable();
            mockSet.Setup(a => a.Find(It.IsAny<int>())).Returns(new Order());

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Orders).Returns(mockSet.Object);

            //Act
            var repository = new OrderRepository(mock.Object);
            repository.Delete(1);

            //Assert
            Mock.Verify(mockSet);
        }
    }
}

