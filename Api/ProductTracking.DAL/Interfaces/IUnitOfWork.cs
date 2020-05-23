using ProductTracking.DAL.Models;
using System;

namespace ProductTracking.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        void Save();
    }
}
