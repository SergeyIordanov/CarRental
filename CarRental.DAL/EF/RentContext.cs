using System.Data.Entity;
using CarRental.Entities.General;

namespace CarRental.DAL.EF
{
    public class RentContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// Static constructor for setting DB initializer
        /// </summary>
        static RentContext()
        {
            Database.SetInitializer(new RentDbInitializer());
        }

        public RentContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
