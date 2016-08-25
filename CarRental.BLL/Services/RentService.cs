using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        public void CreateCar(CarDTO carDto)
        {
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
            // Using ValidationException for transfer validation data to presentation layer
            if (id == null)
                throw new ValidationException("Id is null", "");
            if (!Database.Cars.Find(x => x.Id == id).Any())
                throw new ValidationException("Car wasn't found", "");
            // Deleting car
            Database.Cars.Delete(id.Value);
            //Deleting related orders
            var orders = Database.Orders.GetAll();
            foreach (var order in orders)
            {
                if(order.Car.Id == id)
                    Database.Orders.Delete(order.Id);
            }
            Database.Save();
        }

        public void DeleteReview(int? id)
        {
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
                );
        }

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
