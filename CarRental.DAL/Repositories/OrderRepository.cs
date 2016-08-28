using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;
using NLog;

namespace CarRental.DAL.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly RentContext _db;

        public OrderRepository(RentContext context)
        {
            _db = context;
        }

        public IEnumerable<Order> GetAll()
        {
            Logger.Trace("DAL: OrderRepository.GetAll() called");
            return _db.Orders.Include(x => x.Car).ToList(); 
        }

        public Order Get(int id)
        {
            Logger.Trace("DAL: OrderRepository.Get({0}) called", id);
            return _db.Orders.Find(id);
        }

        public void Create(Order order)
        {
            Logger.Trace("DAL: OrderRepository.Create(order) called");
            _db.Orders.Add(order);
        }

        public void Update(Order order)
        {
            Logger.Trace("DAL: OrderRepository.Update(order) called");
            Order original = _db.Orders.Find(order.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(order);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            Logger.Trace("DAL: OrderRepository.Find() called");
            return _db.Orders.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Logger.Trace("DAL: OrderRepository.Delete({0}) called", id);
            Order order = _db.Orders.Find(id);
            if (order != null)
                _db.Orders.Remove(order);
        }
    }
}
