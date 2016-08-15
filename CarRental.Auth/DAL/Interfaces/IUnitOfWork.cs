using System;
using System.Threading.Tasks;
using CarRental.Auth.DAL.Identity;

namespace CarRental.Auth.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}