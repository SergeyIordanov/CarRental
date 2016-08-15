using System.Data.Entity;
using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Auth.DAL.EF
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    { 
        public AuthContext(string conectionString) : base(conectionString)
        {
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
    }
}
