using CarRental.Auth.DAL.EF;
using CarRental.Auth.DAL.Interfaces;
using CarRental.Entities.Identity;

namespace CarRental.Auth.DAL.Repositories
{
    class ClientManager : IClientManager
    {
        public AuthContext Database { get; set; }

        public ClientManager(AuthContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}