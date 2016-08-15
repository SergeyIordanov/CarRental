using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
