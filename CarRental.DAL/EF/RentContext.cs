using System.Data.Entity;
using CarRental.Entities.General;

namespace CarRental.DAL.EF
{
    public class RentContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        static RentContext()
        {
            Database.SetInitializer<RentContext>(new RentDbInitializer());
        }
        public RentContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
