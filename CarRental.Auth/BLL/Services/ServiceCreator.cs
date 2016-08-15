using CarRental.Auth.BLL.Interfaces;
using CarRental.Auth.DAL.Repositories;

namespace CarRental.Auth.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }
    }
}
