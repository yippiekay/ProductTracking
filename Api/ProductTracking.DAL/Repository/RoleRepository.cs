using Microsoft.EntityFrameworkCore;
using ProductTracking.DAL.EF;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.DAL.Repository
{
    class RoleRepository : IRepository<Role>
    {
        private readonly ProductTrackingDbContext db;

        public RoleRepository(ProductTrackingDbContext context)
        {
            db = context;
        }

        public void Create(Role item)
        {
            db.Roles.Add(item);
        }

        public void Delete(int id)
        {
            var role = db.Roles.Find(id);

            if(role != null)
                db.Roles.Remove(role);
        }

        public void Update(Role item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable <Role> Find(Func<Role, bool> predicate)
        {
            IQueryable<Role> users = db.Roles;
            return users.Where(predicate).ToList();
        }

        public bool Any (int id)
        {
            return db.Roles.Any(c => c.Id == id);
        }

        public Role Get(int id)
        {
            return db.Roles.Find(id);
        }

        public IEnumerable<Role> GetAll()
        {
            return db.Roles.ToList();
        }
    }
}
