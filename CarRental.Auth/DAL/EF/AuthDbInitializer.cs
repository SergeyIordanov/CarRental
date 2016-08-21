using System.Data.Entity;

namespace CarRental.Auth.DAL.EF
{
    /// <summary>
    /// Drops database on each restart of the server
    /// Should be changed/removed before deployment
    /// </summary>
    public class AuthDbInitializer : DropCreateDatabaseAlways<AuthContext>
    {
        protected override void Seed(AuthContext db)
        {
        }
    }
}
