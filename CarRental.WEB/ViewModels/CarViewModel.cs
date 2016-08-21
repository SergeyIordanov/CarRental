using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.WEB.ViewModels
{
    public class CarViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model name is required")]
        [DisplayName("Model")]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public string Class { get; set; }

        [Required(ErrorMessage = "Price for day is required")]
        [DisplayName("Price/day")]
        public decimal PriceForDay { get; set; }

        public byte[] Photo { get; set; }

        public int? Seats { get; set; }

        [Required]
        [DisplayName("Air Conditioning")]
        public bool AirConditioning { get; set; }

        [Required]
        [DisplayName("Automatic transmission")]
        public bool AutomaticTransmission { get; set; }
    }
}