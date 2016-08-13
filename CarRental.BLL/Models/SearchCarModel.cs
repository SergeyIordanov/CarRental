using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.BLL.Models
{
    public class SearchCarModel
    {
        public string[] Brands { get; set; }

        public string[] Classes { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public bool? AirConditioning { get; set; }

        public bool? AutomaticTransmission { get; set; }
    }
}
