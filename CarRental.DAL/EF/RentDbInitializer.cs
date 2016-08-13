using System.Data.Entity;
using CarRental.Entities.General;

namespace CarRental.DAL.EF
{
    /// <summary>
    /// Drops database on each restart of the server and sets initial values to DB
    /// Should be changed/removed before deployment
    /// </summary>
    public class RentDbInitializer : DropCreateDatabaseAlways<RentContext>
    {
        protected override void Seed(RentContext db)
        {
            db.Cars.Add(new Car { Id = 1, AirConditioning = true, AutomaticTransmission = false, Brand = "Renault", Class = "Standard", ModelName = "Duster", PriceForDay = 45M, Seats = 5});
            db.Cars.Add(new Car { Id = 2, AirConditioning = true, AutomaticTransmission = true, Brand = "Renault", Class = "Premium", ModelName = "Fluence", PriceForDay = 73.12M, Seats = null });
            db.Cars.Add(new Car { Id = 3, AirConditioning = false, AutomaticTransmission = false, Brand = "Kia", Class = "Econom", ModelName = "Rio", PriceForDay = 28.5M, Seats = 5 });
            db.Cars.Add(new Car { Id = 4, AirConditioning = true, AutomaticTransmission = true, Brand = "Bmw", Class = "Premium", ModelName = "X6", PriceForDay = 255M, Seats = 5 });
            db.Cars.Add(new Car { Id = 5, AirConditioning = true, AutomaticTransmission = true, Brand = "Lamborghini", Class = "Sport", ModelName = "Gallardo", PriceForDay = 1766M, Seats = 2 });
            db.SaveChanges();
        }
    }
}
