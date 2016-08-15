using System;
using CarRental.Entities.Identity;

namespace CarRental.Auth.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}