using CarRental.Entities.Identity;
using Microsoft.AspNet.Identity;

namespace CarRental.Auth.DAL.Identity
{
    /// <summary>
    /// Identity class for manage users
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        {
        }
    }
}