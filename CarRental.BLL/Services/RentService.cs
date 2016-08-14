using System.Collections.Generic;
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

        public CarDTO GetCar(int? id)
        {
            if (id == null)
                throw new ValidationException("Car's id wasn't set", "");
            var car = Database.Cars.Get(id.Value);
            if (car == null)
                throw new ValidationException("The car wasn't found", "");

            //using of Automapper for projection the Car class on the CarDTO
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<CarDTO>(car);
        }

        public IEnumerable<CarDTO> GetCars()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDTO>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());
        }

        public IEnumerable<CarDTO> GetCars(string searchString)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDTO>());
            var mapper = config.CreateMapper();

            if (string.IsNullOrEmpty(searchString))
                return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.GetAll());

            return mapper.Map<IEnumerable<CarDTO>>(Database.Cars.Find(car => (car.Brand + " " + car.ModelName).ToLower().Contains(searchString.ToLower())));
        }

        public IEnumerable<CarDTO> GetCars(FilterDTO searchModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Car, CarDTO>());
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

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
