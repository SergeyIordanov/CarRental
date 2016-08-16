using System;

namespace CarRental.BLL.DTO
{
    public class OrderDTO
    {
        public enum Status
        {
            Unwatched,
            Declined,
            Accepted,
            Unpaid,
            Paid,
            Returned
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string PickUpAddress { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public decimal TotalPrice { get; set; }

        public bool WithDriver { get; set; }

        public Status OrderStatus { get; set; }

        public string DeclineIssue { get; set; }

        public string UserId { get; set; }

        public virtual CarDTO Car { get; set; }
    }
}
