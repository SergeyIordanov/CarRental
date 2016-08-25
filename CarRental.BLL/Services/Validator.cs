using System.Text.RegularExpressions;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;

namespace CarRental.BLL.Services
{
    public static class Validator
    {
        public static void ValidateCarModel(CarDTO carDto)
        {
            if (carDto == null)
                throw new ValidationException("Cannot create car from null", "");
            if (string.IsNullOrEmpty(carDto.ModelName))
                throw new ValidationException("This property cannot be empty", "ModelName");
            if (string.IsNullOrEmpty(carDto.Brand))
                throw new ValidationException("This property cannot be empty", "Brand");
            if (string.IsNullOrEmpty(carDto.Class))
                throw new ValidationException("This property cannot be empty", "Class");
            if (carDto.Seats != null && carDto.Seats < 0)
                throw new ValidationException("This property cannot be less than 0", "Seats");
        }

        public static void ValidateOrderModel(OrderDTO orderDto)
        {          
            if (orderDto == null)
                throw new ValidationException("Cannot create order from null", "");
            if (orderDto.UserId == null)
                throw new ValidationException("This property cannot be null", "UserId");
            if (string.IsNullOrEmpty(orderDto.FirstName))
                throw new ValidationException("This property cannot be empty", "FirstName");
            if (string.IsNullOrEmpty(orderDto.LastName))
                throw new ValidationException("This property cannot be empty", "LastName");
            if (!Regex.IsMatch(orderDto.PhoneNumber, @"\+38\d\d\d\d\d\d\d\d\d\d"))
                throw new ValidationException("Tel format: +38 xxx xxx xx xx (no spaces)", "PhoneNumber");
            if (string.IsNullOrEmpty(orderDto.PickUpAddress))
                throw new ValidationException("This property cannot be empty", "PickUpAddress");
            if (orderDto.FromDate == null || orderDto.FromDate.Year == 1)
                throw new ValidationException("This property cannot be empty", "FromDate");
            if (orderDto.ToDate == null || orderDto.ToDate.Year == 1)
                throw new ValidationException("This property cannot be empty", "ToDate");
            if (orderDto.FromDate >= orderDto.ToDate)
                throw new ValidationException("Date of drop-off has to be gratter than pick-up date", "FromDate");
        }

        public static void ValidateReviewModel(ReviewDTO reviewDto)
        {
            if (reviewDto == null)
                throw new ValidationException("Cannot create review from null", "");
            if (reviewDto.Text == null)
                throw new ValidationException("This property cannot be null", "Text");
            if (reviewDto.PublishDate == null)
                throw new ValidationException("This property cannot be null", "PublishDate");
        }
    }
}
