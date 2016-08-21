using System;
using CarRental.Entities.Identity;

namespace CarRental.Auth.DAL.Interfaces
{
    /// <summary>
    /// Describes interaction with client profile
    /// </summary>
    public interface IClientManager : IDisposable
    {
        /// <summary>
        /// Creates new client profile
        /// </summary>
        /// <param name="item">Profile to create</param>
        void Create(ClientProfile item);
    }
}