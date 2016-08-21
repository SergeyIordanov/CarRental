namespace CarRental.BLL.DTO
{
    /// <summary>
    /// Class that is used for filtering cars collections
    /// </summary>
    public class FilterDTO
    {
        public string[] Brands { get; set; }

        public string[] Classes { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public bool? AirConditioning { get; set; }

        public bool? AutomaticTransmission { get; set; }
    }
}
