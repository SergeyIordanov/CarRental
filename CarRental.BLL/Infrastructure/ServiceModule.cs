using CarRental.DAL.Interfaces;
using CarRental.DAL.Repositories;
using Ninject.Modules;

namespace CarRental.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;

        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<RentUnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
