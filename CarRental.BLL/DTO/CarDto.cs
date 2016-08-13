namespace CarRental.BLL.DTO
{
    public class CarDTO
    {
        public long Id { get; set; }

        public string Brand { get; set; }

        public string ModelName { get; set; }

        public string Class { get; set; }

        public decimal PriceForDay { get; set; }

        public byte[] Photo { get; set; }

        public int Seats { get; set; }

        public bool AirConditioning { get; set; }

        public bool AutomaticTransmission { get; set; }
    }
}
