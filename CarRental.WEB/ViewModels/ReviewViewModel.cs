using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.WEB.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}