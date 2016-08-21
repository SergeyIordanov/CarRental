using CarRental.Auth.BLL.Interfaces;
using CarRental.Auth.DAL.Repositories;

namespace CarRental.Auth.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            // Here you can change IUnitOfWork implementation & IUserService implementation
            return new UserService(new IdentityUnitOfWork(connection));
        }
    }
}
