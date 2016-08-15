using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;

namespace CarRental.DAL.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly RentContext _db;

        public ReviewRepository(RentContext context)
        {
            _db = context;
        }

        public IEnumerable<Review> GetAll()
        {
            return _db.Reviews;
        }

        public Review Get(int id)
        {
            return _db.Reviews.Find(id);
        }

        public void Create(Review review)
        {
            _db.Reviews.Add(review);
        }

        public void Update(Review review)
        {
            Review original = _db.Reviews.Find(review.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(review);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Review> Find(Func<Review, bool> predicate)
        {
            return _db.Reviews.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Review review = _db.Reviews.Find(id);
            if (review != null)
                _db.Reviews.Remove(review);
        }
    }
}
