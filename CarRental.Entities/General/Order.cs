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

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string PickUpAddress { get; set; }

        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        public bool WithDriver { get; set; }

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
