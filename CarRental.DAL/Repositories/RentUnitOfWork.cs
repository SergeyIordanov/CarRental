using System;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;

namespace CarRental.DAL.Repositories
{
    public class RentUnitOfWork : IUnitOfWork
    {
        private readonly RentContext _db;
        private CarRepository _carRepository;
        private OrderRepository _orderRepository;
        private ReviewRepository _reviewRepository;

        public RentUnitOfWork(string connectionString)
        {
            _db = new RentContext(connectionString);
        }
        public IRepository<Car> Cars => _carRepository ?? (_carRepository = new CarRepository(_db));

        public IRepository<Order> Orders => _orderRepository ?? (_orderRepository = new OrderRepository(_db));

        public IRepository<Review> Reviews => _reviewRepository ?? (_reviewRepository = new ReviewRepository(_db));

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Frees db resources
                    _db.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //All unmanaged resources was disposed so GC needn't to call finalize method
            GC.SuppressFinalize(this);
        }
    }
}
