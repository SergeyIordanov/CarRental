using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarRental.Entities.General;

namespace CarRental.Entities.Identity
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Navigation property for (related to client profile) user
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Navigation property for (related to user) reviews
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
