using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;

namespace CarRental.DAL.Repositories
{
    public class CarRepository : IRepository<Car>
    {
        private readonly RentContext _db;

        public CarRepository(RentContext context)
        {
            this._db = context;
        }

        public IEnumerable<Car> GetAll()
        {
            return _db.Cars;
        }

        public Car Get(int id)
        {
            return _db.Cars.Find(id);
        }

        public void Create(Car car)
        {
            _db.Cars.Add(car);
        }

        public void Update(Car car)
        {
            Car original = _db.Cars.Find(car.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(car);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Car> Find(Func<Car, bool> predicate)
        {
            return _db.Cars.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Car car = _db.Cars.Find(id);
            if (car != null)
                _db.Cars.Remove(car);
        }
    }
}
