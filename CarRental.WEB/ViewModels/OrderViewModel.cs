using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.WEB.ViewModels
{
    public class OrderViewModel
    {
        public enum Status
        {
            Unwatched,
            Declined,
            Accepted,            
            Paid,
            ReturnedWithDamage,
            Returned
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Tel")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Address of pick-up")]
        public string PickUpAddress { get; set; }

        [Required]
        [DisplayName("Pick-up date")]
        public DateTime FromDate { get; set; }

        [Required]
        [DisplayName("Drop-off date")]
        public DateTime ToDate { get; set; }

        [Required]
        [DisplayName("Total price")]
        public decimal TotalPrice { get; set; }

        [Required]
        [DisplayName("With driver")]
        public bool WithDriver { get; set; }

        [Required]
        [DisplayName("Status")]
        public Status OrderStatus { get; set; }

        [DisplayName("Decline issue")]
        public string DeclineIssue { get; set; }

        [DisplayName("Repair price")]
        public decimal RepairPrice { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual CarViewModel Car { get; set; }
    }
}