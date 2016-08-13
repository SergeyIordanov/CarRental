using System;
using CarRental.Entities.General;

namespace CarRental.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Car> Cars { get; }
        void Save();
    }
}
