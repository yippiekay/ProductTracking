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
                    new User(){ 
                    Id = 1, 
                    Email = "bfmvsergey@gmail.com", 
                    Name = "Sergey", 
                    PasswordHash = new byte[] {79, 233, 190, 153, 12, 253, 206, 66, 181, 132, 76, 25, 236, 178, 159, 154, 210, 245, 254, 35},
                    Salt = 1912137082,
                    RoleId = 1}
                });
            modelBuilder.Entity<User>().Property(u => u.RoleId).HasDefaultValue(2);

        }
    }
}
