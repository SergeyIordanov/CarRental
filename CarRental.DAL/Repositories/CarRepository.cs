using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;
using NLog;

namespace CarRental.DAL.Repositories
{
    public class CarRepository : IRepository<Car>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly RentContext _db;

        public CarRepository(RentContext context)
        {
            _db = context;
        }

        public IEnumerable<Car> GetAll()
        {
            Logger.Trace("DAL: CarRepository.GetAll() called");
            return _db.Cars.ToList();
        }

        public Car Get(int id)
        {
            Logger.Trace("DAL: CarRepository.Get({0}) called", id);
            return _db.Cars.Find(id);
        }

        public void Create(Car car)
        {
            Logger.Trace("DAL: CarRepository.Create(car) called");
            _db.Cars.Add(car);
        }

        public void Update(Car car)
        {
            Logger.Trace("DAL: CarRepository.Update(car) called");
            Car original = _db.Cars.Find(car.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(car);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Car> Find(Func<Car, bool> predicate)
        {
            Logger.Trace("DAL: CarRepository.Find() called");
            return _db.Cars.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Logger.Trace("DAL: CarRepository.Delete({0}) called", id);
            Car car = _db.Cars.Find(id);
            if (car != null)
                _db.Cars.Remove(car);
        }
    }
}
