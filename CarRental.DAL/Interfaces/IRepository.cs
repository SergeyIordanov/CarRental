using System;
using System.Collections.Generic;

namespace CarRental.DAL.Interfaces
{
    /// <summary>
    /// Interface for implementation of generic repository pattern
    /// </summary>
    /// <typeparam name="T">Class of entitiy repository will work with</typeparam>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Get(int id);

        IEnumerable<T> Find(Func<T, bool> predicate);

        void Create(T item);

        void Update(T item);

        void Delete(int id);
    }
}
