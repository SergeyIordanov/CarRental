using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Entities.General
{
    public class Review
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// User id from AuthDB
        /// </summary>
        public string UserId { get; set; }
    }
}
