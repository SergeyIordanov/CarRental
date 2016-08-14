using System.Data.Entity;
using System.IO;
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
            const string duster = @"C:\1Sergey\4ProjectsInGit\CarRental\CarRental.WEB\Content\Images\Renault-Duster.jpg";
            const string fluence = @"C:\1Sergey\4ProjectsInGit\CarRental\CarRental.WEB\Content\Images\Renault-Fluence.jpg";
            const string rio = @"C:\1Sergey\4ProjectsInGit\CarRental\CarRental.WEB\Content\Images\Kia-Rio.jpg";
            const string x6 = @"C:\1Sergey\4ProjectsInGit\CarRental\CarRental.WEB\Content\Images\Bmw-X6.jpg";
            const string gallardo = @"C:\1Sergey\4ProjectsInGit\CarRental\CarRental.WEB\Content\Images\Lamborghini-Gallardo.jpg";

            db.Cars.Add(new Car { Id = 1, AirConditioning = true, AutomaticTransmission = false, Brand = "Renault", Class = "Standard", ModelName = "Duster", PriceForDay = 45M, Seats = 5, Photo = GetImage(duster)});
            db.Cars.Add(new Car { Id = 2, AirConditioning = true, AutomaticTransmission = true, Brand = "Renault", Class = "Premium", ModelName = "Fluence", PriceForDay = 73.12M, Seats = 5, Photo = GetImage(fluence) });
            db.Cars.Add(new Car { Id = 3, AirConditioning = false, AutomaticTransmission = false, Brand = "Kia", Class = "Econom", ModelName = "Rio", PriceForDay = 28.5M, Seats = 5, Photo = GetImage(rio) });
            db.Cars.Add(new Car { Id = 4, AirConditioning = true, AutomaticTransmission = true, Brand = "Bmw", Class = "Premium", ModelName = "X6", PriceForDay = 255M, Seats = 5, Photo = GetImage(x6) });
            db.Cars.Add(new Car { Id = 5, AirConditioning = true, AutomaticTransmission = true, Brand = "Lamborghini", Class = "Sport", ModelName = "Gallardo", PriceForDay = 1766M, Seats = 2, Photo = GetImage(gallardo) });
            db.SaveChanges();
        }

        private byte[] GetImage(string path)
        {
            byte[] result;
            using (var fs = new FileStream(path, FileMode.Open))
            {
                result = new byte[fs.Length];
                fs.Read(result, 0, result.Length);
            }
            return result;
        }
    }
}
