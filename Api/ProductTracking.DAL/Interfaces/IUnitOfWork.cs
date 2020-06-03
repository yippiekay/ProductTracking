using ProductTracking.DAL.Models;
using System;

namespace ProductTracking.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Task> Tasks { get; }
        void Save();
    }
}
