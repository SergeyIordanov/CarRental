namespace CarRental.WEB.ViewModels
{
    public class FilterViewModel
    {
        public string[] Brands { get; set; }

        public string[] Classes { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public bool? AirConditioning { get; set; }

        public bool? AutomaticTransmission { get; set; }
    }
}