using JWTAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {

        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(e => e.Email).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.Password).IsRequired();
            modelBuilder.Entity<User>().Property(e => e.Username).IsRequired();
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(e => e.Email).IsUnique();
                e.HasIndex(e => e.Username).IsUnique();
            });
        }

    }
}
