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
    public class ReviewRepositoryTest
    {
        [Test, TestCaseSource(typeof(TestData), nameof(TestData.ReviewsData))]
        public void GetAllReturnsReviewsList(IQueryable<Review> data)
        {
            //Arrange                       
            var mockSet = new Mock<DbSet<Review>>();
            mockSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Reviews).Returns(mockSet.Object);

            //Act
            var repository = new ReviewRepository(mock.Object);
            var result = repository.GetAll();

            //Assert
            Assert.That(result, Is.TypeOf(typeof(List<Review>)));
        }

        [Test]
        public void GetReturnsReviewOnCorrectId()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Review>>();

            mockSet.Setup(a => a.Find(It.IsAny<int>())).Returns(new Review());

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Reviews).Returns(mockSet.Object);

            //Act
            var repository = new ReviewRepository(mock.Object);
            var result = repository.Get(1);

            //Assert
            Assert.That(result, Is.TypeOf(typeof(Review)));
        }

        [Test]
        public void CreateCallsAdd()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Review>>();

            mockSet.Setup(a => a.Add(It.IsAny<Review>())).Verifiable();

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Reviews).Returns(mockSet.Object);

            //Act
            var repository = new ReviewRepository(mock.Object);
            repository.Create(new Review());

            //Assert
            Mock.Verify(mockSet);
        }

        [Test, TestCaseSource(typeof(TestData), nameof(TestData.ReviewsData))]
        public void FindReturnsCorrectCarsList(IQueryable<Review> data)
        {
            //Arrange                       
            var mockSet = new Mock<DbSet<Review>>();
            mockSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Reviews).Returns(mockSet.Object);

            //Act
            var repository = new ReviewRepository(mock.Object);
            var result = repository.Find(review => review.Text.Contains("simple"));

            //Assert
            Assert.That(result, Has.Count.EqualTo(2).And.TypeOf(typeof(List<Review>)));
        }

        [Test]
        public void DeleteCallRemoveOnCorrectId()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Review>>();

            mockSet.Setup(a => a.Remove(It.IsAny<Review>())).Verifiable();
            mockSet.Setup(a => a.Find(It.IsAny<int>())).Returns(new Review());

            var mock = new Mock<RentContext>();
            mock.Setup(a => a.Reviews).Returns(mockSet.Object);

            //Act
            var repository = new ReviewRepository(mock.Object);
            repository.Delete(1);

            //Assert
            Mock.Verify(mockSet);
        }
    }
}
