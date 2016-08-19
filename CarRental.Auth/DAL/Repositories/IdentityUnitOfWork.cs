using System;
using System.Threading.Tasks;
using CarRental.Auth.DAL.EF;
using CarRental.Auth.DAL.Identity;
using CarRental.Auth.DAL.Interfaces;
using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Auth.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private readonly AuthContext _db;

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly IClientManager _clientManager;

        public IdentityUnitOfWork(string connectionString)
        {
            _db = new AuthContext(connectionString);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _clientManager = new ClientManager(_db);
        }

        public ApplicationUserManager UserManager => _userManager;

        public IClientManager ClientManager => _clientManager;

        public ApplicationRoleManager RoleManager => _roleManager;

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _roleManager.Dispose();
                    _clientManager.Dispose();
                }
                _disposed = true;
            }
        }
    }
}