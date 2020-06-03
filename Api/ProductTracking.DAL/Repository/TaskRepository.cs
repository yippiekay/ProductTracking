using Microsoft.EntityFrameworkCore;
using ProductTracking.DAL.EF;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.DAL.Repository
{
    class TaskRepository : IRepository<Task>
    {
        private readonly ProductTrackingDbContext db;

        public TaskRepository(ProductTrackingDbContext context)
        {
            db = context;
        }

        public void Create(Task item)
        {
            db.Tasks.Add(item);
        }

        public void Update(Task item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var task = db.Tasks.Find(id);
            
            if(task != null)
                db.Tasks.Remove(task);
        }

        public bool Any(int id)
        {
            return db.Tasks.Any(t => t.Id == id);
        }

        public IEnumerable<Task> Find(Func<Task, bool> predicate)
        {
            return db.Tasks.Where(predicate).ToList();
        }
        
        public Task Get(int id)
        {
            return db.Tasks.Find(id);
        }

        public IEnumerable<Task> GetAll()
        {
            return db.Tasks.ToList();
        }
    }
}
