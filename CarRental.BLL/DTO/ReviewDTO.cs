using System;

namespace CarRental.BLL.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime PublishDate { get; set; }

        public string UserId { get; set; }
    }
}
