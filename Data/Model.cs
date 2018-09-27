using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var admin = new IdentityRole("admin");
            var user = new IdentityRole("user");

            var test = new User { UserName = "test", PasswordHash = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8" };
            var test2 = new User { UserName = "test2", PasswordHash = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8" };

            modelBuilder.Entity<IdentityRole>().HasData(
                admin, user
            );
            modelBuilder.Entity<User>().HasData(test, test2);

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, UserId = test.Id, RoleId = admin.Id },
                new UserRole { Id = 2, UserId = test.Id, RoleId = user.Id },
                new UserRole { Id = 3, UserId = test2.Id, RoleId = user.Id }
            );
        }

    }

    public class User : IdentityUser
    {
    }

    public class UserRole
    {
        public int Id { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityRole Role { get; set; }
        [ForeignKey(nameof(Role))]
        public string RoleId { get; set; }
    }
}