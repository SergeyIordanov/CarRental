using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;

namespace CarRental.DAL.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly RentContext _db;

        public OrderRepository(RentContext context)
        {
            _db = context;
        }

        public IEnumerable<Order> GetAll()
        {
            return _db.Orders.ToList();
        }

        public Order Get(int id)
        {
            return _db.Orders.Find(id);
        }

        public void Create(Order order)
        {
            _db.Orders.Add(order);
        }

        public void Update(Order order)
        {
            Order original = _db.Orders.Find(order.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(order);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Order> Find(Func<Order, bool> predicate)
        {
            return _db.Orders.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Order order = _db.Orders.Find(id);
            if (order != null)
                _db.Orders.Remove(order);
        }
    }
}
