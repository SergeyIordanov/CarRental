using System.Data.Entity;
using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Auth.DAL.EF
{
    /// <summary>
    /// Context for working with related database
    /// </summary>
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Static constructor for setting DB initializer
        /// </summary>
        static AuthContext()
        {
            Database.SetInitializer(new AuthDbInitializer());
        }

        public AuthContext(string conectionString) : base(conectionString)
        {
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
    }
}
