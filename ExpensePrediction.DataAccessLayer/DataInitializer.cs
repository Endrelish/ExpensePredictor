using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpensePrediction.DataAccessLayer
{
    public static class DataInitializer
    {
        public static void Initialize(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseCategory>().HasData(
                new ExpenseCategory()
                {
                    Id = "27a62b11-7765-473d-a1b6-7ecda9915dd5",
                    Name = "Żarcie"
                },
                new ExpenseCategory()
                {
                    Id = "7edeb282-6172-49b1-9a8f-5c3b25b7d0b8",
                    Name = "Alkohol"
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "830f0d08-6a56-4a57-83c9-329c094f184b",
                    FirstName = "User",
                    LastName = "User",
                    UserName = "user",
                    NormalizedUserName = "USER",
                    Email = "user@user.com",
                    NormalizedEmail = "USER@USER.COM",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEODZFtx31yVlQlAo6GcTs2dIyi/Dcch0/uqv27PvT/xXzy5+JAZEMVS5SvM13yrMdQ==",
                    PhoneNumber = "123456780"
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = "2fdbec88-4aa9-430c-8359-8b27756cf1ca",
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = "307d5d07-87cf-49a0-9cd4-b925b5380963",
                    Name = "user",
                    NormalizedName = "USER"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                    RoleId = "2fdbec88-4aa9-430c-8359-8b27756cf1ca"
                },
                new IdentityUserRole<string>
                {
                    UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                    RoleId = "307d5d07-87cf-49a0-9cd4-b925b5380963"
                }
            );
        }
    }
}