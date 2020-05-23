using Microsoft.EntityFrameworkCore;
using ProductTracking.DAL.EF;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            db.Add(item);
        }

        public void Delete(int id)
        {
            var role = db.Roles.Find(id);
            if(role != null)
            {
                db.Roles.Remove(role);
            }
        }

        public IEnumerable<Role> Find(Func<Role, bool> predicate)
        {
            return db.Roles.Where(predicate).ToList();
        }

        public Role Get(int id)
        {
            return db.Roles.Find(id);
        }

        public IEnumerable<Role> GetAll()
        {
            return db.Roles.ToList();
        }

        public void Update(Role item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
