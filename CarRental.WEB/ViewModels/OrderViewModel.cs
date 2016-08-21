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

        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DisplayName("Tel")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address of pick-up is required")]
        [DisplayName("Address of pick-up")]
        public string PickUpAddress { get; set; }

        [Required(ErrorMessage = "Pick-up date is required")]
        [DisplayName("Pick-up date")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Drop-off date is required")]
        [DisplayName("Drop-off date")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Total price is required")]
        [DisplayName("Total price")]
        public decimal TotalPrice { get; set; }

        [Required]
        [DisplayName("With driver")]
        public bool WithDriver { get; set; }

        [Required(ErrorMessage = "Status is required")]
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