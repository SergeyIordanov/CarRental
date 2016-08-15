using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Entities.General
{
    public class Review
    {
        public int Id { get; set; }

        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        public string UserId { get; set; }
    }
}
