using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Entities.Identity
{
    /// <summary>
    /// Identity entity for storing users
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Navigation property for (related to user) client profile
        /// </summary>
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
