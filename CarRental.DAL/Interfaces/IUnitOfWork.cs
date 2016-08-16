using System;
using CarRental.Entities.General;

namespace CarRental.DAL.Interfaces
{
    /// <summary>
    /// Interface for implementation of unit of work pattern.
    /// Presantation layer will work with BLL using this one.
    /// Simply: unite all the implementations of generic repository together in one place.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Car> Cars { get; }

        IRepository<Order> Orders { get; }

        IRepository<Review> Reviews { get; }
       
        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        void Save();
    }
}
