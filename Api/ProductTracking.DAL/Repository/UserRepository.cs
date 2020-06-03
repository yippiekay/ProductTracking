using Microsoft.EntityFrameworkCore;
using ProductTracking.DAL.EF;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly ProductTrackingDbContext db;

        public UserRepository(ProductTrackingDbContext context)
        {
            db = context;
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var user = db.Users.Find(id);

            if (user != null)
                db.Users.Remove(user);
        }

        public bool Any(int id)
        {
            return db.Users.Any(c => c.Id == id);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public User Get(int id)
        {
            return db.Users.Include(u => u.Tasks).Include(u => u.Role).Where(u => u.Id == id).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.Include(u => u.Tasks).Include(u => u.Role).ToList();
        }
    }
}
