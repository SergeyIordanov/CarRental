using System;
using CarRental.Auth.DAL.Identity;

namespace CarRental.Auth.DAL.Interfaces
{
    /// <summary>
    /// Interface for implementation of unit of work pattern.
    /// Presantation layer will work with Auth.BLL using this interface.
    /// Simply: unite all the implementations of generic repository together in one place.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Access to class with user management
        /// </summary>
        ApplicationUserManager UserManager { get; }

        /// <summary>
        /// Access to class with user's client profiles management
        /// </summary>
        IClientManager ClientManager { get; }

        /// <summary>
        /// Access to class with roles management
        /// </summary>
        ApplicationRoleManager RoleManager { get; }

        void Save();
    }
}