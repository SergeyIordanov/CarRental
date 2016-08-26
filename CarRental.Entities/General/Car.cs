using System.ComponentModel.DataAnnotations;

namespace CarRental.Entities.General
{
    public class Car
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal PriceForDay { get; set; }

        [DataType(DataType.Upload)]
        public byte[] Photo { get; set; }

        public int? Seats { get; set; }

        public bool AirConditioning { get; set; }

        public bool AutomaticTransmission { get; set; }
    }
}
