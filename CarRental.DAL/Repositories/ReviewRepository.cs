using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.DAL.EF;
using CarRental.DAL.Interfaces;
using CarRental.Entities.General;
using NLog;

namespace CarRental.DAL.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly RentContext _db;

        public ReviewRepository(RentContext context)
        {
            _db = context;
        }

        public IEnumerable<Review> GetAll()
        {
            Logger.Trace("DAL: ReviewRepository.GetAll() called");
            return _db.Reviews.ToList();
        }

        public Review Get(int id)
        {
            Logger.Trace("DAL: ReviewRepository.Get({0}) called", id);
            return _db.Reviews.Find(id);
        }

        public void Create(Review review)
        {
            Logger.Trace("DAL: ReviewRepository.Create(review) called");
            _db.Reviews.Add(review);
        }

        public void Update(Review review)
        {
            Logger.Trace("DAL: ReviewRepository.Update(review) called");
            Review original = _db.Reviews.Find(review.Id);
            if (original != null)
            {
                _db.Entry(original).CurrentValues.SetValues(review);
                _db.SaveChanges();
            }
        }

        public IEnumerable<Review> Find(Func<Review, bool> predicate)
        {
            Logger.Trace("DAL: ReviewRepository.Find() called");
            return _db.Reviews.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Logger.Trace("DAL: ReviewRepository.Delete({0}) called", id);
            Review review = _db.Reviews.Find(id);
            if (review != null)
                _db.Reviews.Remove(review);
        }
    }
}
