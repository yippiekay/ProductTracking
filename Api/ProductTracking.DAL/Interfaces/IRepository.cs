using System;
using System.Collections.Generic;

namespace ProductTracking.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        bool Any(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
