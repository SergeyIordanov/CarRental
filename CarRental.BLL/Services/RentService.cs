using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;

namespace CarRental.BLL.Services
{
    public class RentService : IRentService
    {
        private IUnitOfWork Database { get; }

        public RentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        #region Create

        public void CreateReview(ReviewDTO reviewDto)
        {
            if (reviewDto == null)
                throw new ValidationException("Cannot create review from null", "");
            if (reviewDto.Text == null)
                throw new ValidationException("This property cannot be null", "Text");
            if (reviewDto.PublishDate == null)
                throw new ValidationException("This property cannot be null", "PublishDate");
            Mapper.Initialize(cfg => cfg.CreateMap<ReviewDTO, Review>());
            var review = Mapper.Map<Review>(reviewDto);
            Database.Reviews.Create(review);
            Database.Save();
        }

        public void CreateOrder(OrderDTO orderDto, int? carId)
        {
            if (carId == null)
                throw new ValidationException("Car's id wasn't set", "");
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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<CarDTO, Car>();
            });
            var mapper = config.CreateMapper();
            var order = mapper.Map<Order>(orderDto);
            var car = Database.Cars.Get(carId.Value);
            if (car == null)
                throw new ValidationException("The car wasn't found", "");
            order.Car = car;
            order.TotalPrice = (order.ToDate.ToUniversalTime() - order.FromDate.ToUniversalTime()).Days *
                                      order.Car.PriceForDay;
            if (order.WithDriver)
                order.TotalPrice += 20 *
                                       (order.ToDate.ToUniversalTime() - order.FromDate.ToUniversalTime()).Days;
            Database.Orders.Create(order);
            Database.Save();
        }

        #endregion

        #region Update

        public void UpdateCar(CarDTO carDto)
        {
            if (Database.Cars.Get(carDto.Id) == null)
                throw new ValidationException("Car wasn't found", "");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CarDTO, Car>();
            });
            var mapper = config.CreateMapper();
            var car = mapper.Map<Car>(carDto);

            Database.Cars.Update(car);

            Database.Save();
        }

        public void UpdateOrder(OrderDTO orderDto)
        {
            if (Database.Orders.Get(orderDto.Id) == null)
                throw new ValidationException("Order wasn't found", "");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<CarDTO, Car>();
            });
            var mapper = config.CreateMapper();
            var order = mapper.Map<Order>(orderDto);

            Database.Orders.Update(order);

            Database.Save();
        }

        #endregion

        #region Delete

        public void DeleteCar(int? id)
        {
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Cars.Find(x => x.Id == id).Any())
                throw new ValidationException("Car wasn't found", "");
            Database.Cars.Delete(id.Value);
            Database.Save();
        }

        public void DeleteReview(int? id)
        {
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Reviews.Find(x => x.Id == id).Any())
                throw new ValidationException("Review wasn't found", "");
            Database.Reviews.Delete(id.Value);
            Database.Save();
        }

        public void DeleteOrder(int? id)
        {
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Orders.Find(x => x.Id == id).Any())
                throw new ValidationException("Order wasn't found", "");
            Database.Orders.Delete(id.Value);
            Database.Save();
        }

        #endregion

        #region Get

        public CarDTO GetCar(int? id)
        {
            if (id == null)
                throw new ValidationException("Car's id wasn't set", "");
            var car = Database.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("The car wasn't found", "");

            //using of Automapper for projection the Car class on the CarDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<CarDTO>(car);
        }

        public IEnumerable<CarDTO> GetCars()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());
        }

        public IEnumerable<CarDTO> GetCars(string searchString)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = config.CreateMapper();

            if (string.IsNullOrEmpty(searchString))
                return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());

            return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.Find(car => (car.Brand + " " + car.ModelName).ToLower().Contains(searchString.ToLower())));
        }

        public IEnumerable<CarDTO> GetCars(FilterDTO searchModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = config.CreateMapper();

            if (searchModel == null)
                return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());

            // compare each car with searchModel
            return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.Find(
                car => (searchModel.Brands == null || searchModel.Brands.Length < 1 || searchModel.Brands.Contains(car.Brand)) &&
                       (searchModel.Classes == null || searchModel.Classes.Length < 1 || searchModel.Classes.Contains(car.Class)) &&
                       searchModel.MinPrice < car.PriceForDay &&
                       searchModel.MaxPrice > car.PriceForDay &&
                       (searchModel.AirConditioning == null || searchModel.AirConditioning == car.AirConditioning) &&
                       (searchModel.AutomaticTransmission == null || searchModel.AutomaticTransmission == car.AutomaticTransmission)
                ));
        }

        public IEnumerable<ReviewDTO> GetReviews()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Review, ReviewDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ReviewDTO>>(Database.Reviews.GetAll());
        }

        public OrderDTO GetOrder(int? id)
        {
            if (id == null)
                throw new ValidationException("Order's id wasn't set", "");
            var order = Database.Orders.Get(id.Value);
            if (order == null)
                throw new ValidationException("The order wasn't found", "");

            //using of Automapper for projection the Order class on the OrderDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<Car, CarDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<OrderDTO>(order);
        }

        public IEnumerable<OrderDTO> GetOrders()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<Car, CarDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<OrderDTO>>(Database.Orders.GetAll());
        }

        public IEnumerable<OrderDTO> GetOrders(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("User's id wasn't set", "");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<Car, CarDTO>();
            });
            var mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<OrderDTO>>(Database.Orders.Find(order => order.UserId.Equals(userId)));
        }

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
