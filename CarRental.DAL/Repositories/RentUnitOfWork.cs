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

        public RentUnitOfWork(string connectionString)
        {
            _db = new RentContext(connectionString);
        }
        public IRepository<Car> Cars => _carRepository ?? (_carRepository = new CarRepository(_db));

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
                    _db.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
