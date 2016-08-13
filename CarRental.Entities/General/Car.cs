using System.ComponentModel.DataAnnotations;

namespace CarRental.Entities.General
{
    public class Car
    {
        public long Id { get; set; }

        public string Brand { get; set; }

        public string ModelName { get; set; }

        public string Class { get; set; }

        [DataType(DataType.Currency)]
        public decimal PriceForDay { get; set; }

        [DataType(DataType.Upload)]
        public byte[] Photo { get; set; }

        public int Seats { get; set; }

        public bool AirConditioning { get; set; }

        public bool AutomaticTransmission { get; set; }
    }
}
