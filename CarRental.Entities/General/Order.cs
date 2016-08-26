using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Entities.General
{
    public class Order
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
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public string PickUpAddress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [Required]
        public bool WithDriver { get; set; }

        [Required]
        public Status OrderStatus { get; set; }

        [DataType(DataType.MultilineText)]
        public string DeclineIssue { get; set; }

        [DataType(DataType.Currency)]
        public decimal RepairPrice { get; set; }

        /// <summary>
        /// User id from AuthDB
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Navigation property for related car
        /// </summary>
        public virtual Car Car { get; set; }
    }
}
