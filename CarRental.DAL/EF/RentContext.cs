using System.Data.Entity;
using CarRental.Entities.General;

namespace CarRental.DAL.EF
{
    /// <summary>
    /// Context for working with related database
    /// </summary>
    public class RentContext : DbContext
    {
        public virtual DbSet<Car> Cars { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// Static constructor for setting DB initializer
        /// </summary>
        static RentContext()
        {
            Database.SetInitializer(new RentDbInitializer());
        }

        public RentContext() { }

        public RentContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
               .HasMany(a => a.Orders)
               .WithRequired(a => a.Car)
               .WillCascadeOnDelete(true);
        }
    }
}
