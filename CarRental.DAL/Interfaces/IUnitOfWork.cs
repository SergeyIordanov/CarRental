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
        /// <summary>
        /// Gives access to cars repository
        /// </summary>
        IRepository<Car> Cars { get; }

        /// <summary>
        /// Gives access to orders repository
        /// </summary>
        IRepository<Order> Orders { get; }

        /// <summary>
        /// Gives access to reviews repository
        /// </summary>
        IRepository<Review> Reviews { get; }
       
        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        void Save();
    }
}
