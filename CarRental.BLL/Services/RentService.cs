using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using CarRental.BLL.DTO;
using CarRental.BLL.Infrastructure;
using CarRental.BLL.Interfaces;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;
using NLog;

namespace CarRental.BLL.Services
{
    public class RentService : IRentService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IUnitOfWork Database { get; }

        public RentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        #region Create

        public void CreateCar(CarDTO carDto)
        {
            Logger.Debug("BLL: CreateCar() called");
            // Using ValidationException for transfer validation data to presentation layer
            Validator.ValidateCarModel(carDto);
            // Mapping DTO object into DB entity
            Mapper.Initialize(cfg => cfg.CreateMap<CarDTO, Car>());
            var car = Mapper.Map<Car>(carDto);
            // Creating and saving
            Database.Cars.Create(car);
            Database.Save();
        }

        public void CreateReview(ReviewDTO reviewDto)
        {
            Logger.Debug("BLL: CreateReview() called");
            // Using ValidationException for transfer validation data to presentation layer
            Validator.ValidateReviewModel(reviewDto);
            // Mapping DTO object into DB entity
            Mapper.Initialize(cfg => cfg.CreateMap<ReviewDTO, Review>());
            var review = Mapper.Map<Review>(reviewDto);
            // Creating and saving
            Database.Reviews.Create(review);
            Database.Save();
        }

        public void CreateOrder(OrderDTO orderDto, int? carId)
        {
            Logger.Debug("BLL: CreateOrder() called");
            // Using ValidationException for transfer validation data to presentation layer
            if (carId == null)
                throw new ValidationException("Car's id wasn't set", "");
            Validator.ValidateOrderModel(orderDto);
            // Mapping DTO object into DB entity
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<CarDTO, Car>(); // used for inserted mapping
            });
            var mapper = config.CreateMapper();
            var order = mapper.Map<Order>(orderDto);

            // Setting neccessary fields
            var car = Database.Cars.Get(carId.Value);
            if (car == null)
                throw new ValidationException("The car wasn't found", "");
            order.Car = car;
            order.TotalPrice = (order.ToDate.ToUniversalTime() - order.FromDate.ToUniversalTime()).Days *
                                      order.Car.PriceForDay;
            if (order.WithDriver)
                order.TotalPrice += 20 *
                                       (order.ToDate.ToUniversalTime() - order.FromDate.ToUniversalTime()).Days;
            // Creating and saving
            Database.Orders.Create(order);
            Database.Save();
        }

        #endregion

        #region Update

        public void UpdateCar(CarDTO carDto)
        {
            Logger.Debug("BLL: UpdateCar() called");
            // Using ValidationException for transfer validation data to presentation layer
            if (Database.Cars.Get(carDto.Id) == null)
                throw new ValidationException("Car wasn't found", "");
            Validator.ValidateCarModel(carDto);
            // Mapping DTO object into DB entity
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CarDTO, Car>(); });
            var mapper = config.CreateMapper();
            var car = mapper.Map<Car>(carDto);
            // Updating & saving
            Database.Cars.Update(car);
            Database.Save();
        }

        public void UpdateOrder(OrderDTO orderDto)
        {
            Logger.Debug("BLL: UpdateOrder() called");
            // Using ValidationException for transfer validation data to presentation layer
            if (Database.Orders.Get(orderDto.Id) == null)
                throw new ValidationException("Order wasn't found", "");
            Validator.ValidateOrderModel(orderDto);
            // Mapping DTO object into DB entity
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<CarDTO, Car>();
            });
            var mapper = config.CreateMapper();
            var order = mapper.Map<Order>(orderDto);
            // Updating & saving
            Database.Orders.Update(order);
            Database.Save();
        }

        #endregion

        #region Delete

        public void DeleteCar(int? id)
        {
            Logger.Debug("BLL: DeleteCar({0}) called", id);
            // Using ValidationException for transfer validation data to presentation layer
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Cars.Find(x => x.Id == id).Any())
                throw new ValidationException("Car wasn't found", "");
            // Deleting & saving
            Database.Cars.Delete(id.Value);
            Database.Save();
        }

        public void DeleteReview(int? id)
        {
            Logger.Debug("BLL: DeleteReview({0}) called", id);
            // Using ValidationException for transfer validation data to presentation layer
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Reviews.Find(x => x.Id == id).Any())
                throw new ValidationException("Review wasn't found", "");
            // Deleting & saving
            Database.Reviews.Delete(id.Value);
            Database.Save();
        }

        public void DeleteOrder(int? id)
        {
            Logger.Debug("BLL: DeleteOrder({0}) called", id);
            // Using ValidationException for transfer validation data to presentation layer
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Orders.Find(x => x.Id == id).Any())
                throw new ValidationException("Order wasn't found", "");
            // Deleting & saving
            Database.Orders.Delete(id.Value);
            Database.Save();
        }

        #endregion

        #region Get

        public CarDTO GetCar(int? id)
        {
            Logger.Debug("BLL: GetCar({0}) called", id);
            // Using ValidationException for transfer validation data to presentation layer
            if (id == null)
                throw new ValidationException("Car's id wasn't set", "");
            var car = Database.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("The car wasn't found", "");

            // Using of Automapper for projection the Car entity into the CarDTO
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
            Logger.Debug("BLL: GetCars() called");
            // Using of Automapper for projection the Car entity into the CarDTO
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
            Logger.Debug("BLL: GetCars({0}) called", searchString);
            // Using of Automapper for projection the Car entity into the CarDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = config.CreateMapper();
            // Returns all items if search string is empty or null
            if (string.IsNullOrEmpty(searchString))
                return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());

            return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.Find(car => (car.Brand + " " + car.ModelName).ToLower().Contains(searchString.ToLower())));
        }

        public IEnumerable<CarDTO> GetCars(FilterDTO searchModel)
        {
            Logger.Debug("BLL: GetCars(filter) called");
            // Using of Automapper for projection the Car entity into the CarDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Car, CarDTO>();
                cfg.CreateMap<Order, OrderDTO>();
            });
            var mapper = config.CreateMapper();

            if (searchModel == null)
                return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());

            // Comparing each car with searchModel
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
            Logger.Debug("BLL: GetReviews() called");
            // Using of Automapper for projection the Review entity into the ReviewDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Review, ReviewDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ReviewDTO>>(Database.Reviews.GetAll());
        }

        public OrderDTO GetOrder(int? id)
        {
            Logger.Debug("BLL: GetOrder({0}) called", id);
            // Using ValidationException for transfer validation data to presentation layer
            if (id == null)
                throw new ValidationException("Order's id wasn't set", "");
            var order = Database.Orders.Get(id.Value);
            if (order == null)
                throw new ValidationException("The order wasn't found", "");

            //Using of Automapper for projection the Order entity on the OrderDTO
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
            Logger.Debug("BLL: GetOrders() called");
            //Using of Automapper for projection the Order entity on the OrderDTO
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
            Logger.Debug("BLL: GetOrders({0}) called", userId);
            // Using ValidationException for transfer validation data to presentation layer
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("User's id wasn't set", "");
            // Using of Automapper for projection the Order entity on the OrderDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<Car, CarDTO>(); // used for inserted mapping
            });
            var mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<OrderDTO>>(Database.Orders.Find(order => order.UserId.Equals(userId)));
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")] // all possible NullReferenceExceptions checked
        public IEnumerable<OrderDTO> GetOrders(string searchCar, string searchUser)
        {
            Logger.Debug("BLL: GetOrders({0}, {1}) called", searchCar, searchUser);
            // Using of Automapper for projection the Order entity on the OrderDTO
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<Car, CarDTO>();
            });
            var mapper = config.CreateMapper();

            var orders = mapper.Map<IEnumerable<OrderDTO>>(Database.Orders.GetAll());

            bool isCar = !string.IsNullOrEmpty(searchCar); // search by car?
            bool isUser = !string.IsNullOrEmpty(searchUser); // search by user?

            return orders.Select(x => x).Where(order =>
                (!isCar || (order.Car.Brand + " " + order.Car.ModelName).ToLower().Contains(searchCar.ToLower())) &&
                (!isUser || (order.FirstName + " " + order.LastName).ToLower().Contains(searchUser.ToLower()))
                ).ToList();
        }

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
