using Microsoft.EntityFrameworkCore;
using ProductTracking.DAL.Models;

namespace ProductTracking.DAL.EF
{
    public class ProductTrackingDbContext : DbContext
    {
        public ProductTrackingDbContext(DbContextOptions<ProductTrackingDbContext> option) : base(option)
        {
            Database.EnsureCreated();
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role []
                {
                    new Role(){ Id = 1, Name = "admin"},
                    new Role(){ Id = 2, Name = "user"}
                });
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User(){ Email = "bfmvsergey@gmail.com", Name = "Sergey", Password = "1234qwer", UserId = 1, RoleId = 1},
                    new User(){ Email = "ex", Name = "Lex", Password = "1234", UserId = 2}
                });
            modelBuilder.Entity<User>().Property(u => u.RoleId).HasDefaultValue(2);

        }
    }
}
