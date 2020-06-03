using ProductTracking.DAL.EF;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;

namespace ProductTracking.DAL.Repository
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ProductTrackingDbContext db;
        private UserRepository userRepository;
        private RoleRepository roleRepository;
        private TaskRepository taskRepository;

        public EFUnitOfWork(ProductTrackingDbContext context)
        {
            db = context; 
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(db);
                return roleRepository;
            }
        }

        public IRepository<Task> Tasks
        {
            get 
            {
                if (taskRepository == null)
                    taskRepository = new TaskRepository(db);
                return taskRepository;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    db.Dispose();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
