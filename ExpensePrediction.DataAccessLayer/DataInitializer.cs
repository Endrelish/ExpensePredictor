using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExpensePrediction.DataAccessLayer
{
    public static class DataInitializer
    {
        public static void Initialize(ModelBuilder modelBuilder)
        {
            var expenseCategories = new []
            {
                new ExpenseCategory
                {
                    Id = "27a62b11-7765-473d-a1b6-7ecda9915dd5",
                    Name = "Żarcie"
                },
                new ExpenseCategory
                {
                    Id = "7edeb282-6172-49b1-9a8f-5c3b25b7d0b8",
                    Name = "Alkohol"
                },
                new ExpenseCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Samochód"
                }
            };
            modelBuilder.Entity<ExpenseCategory>().HasData(expenseCategories);

            var incomeCategories = new[]
            {
                new IncomeCategory
                {
                    Id = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                    Name = "Przychód regularny"
                },
                new IncomeCategory
                {
                    Id = "10c0cb83-1cb2-4c24-84ca-1757225bb97d",
                    Name = "Przychód jednorazowy"
                }
            };
            modelBuilder.Entity<IncomeCategory>().HasData(incomeCategories);

            var user = new User
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
            };
            modelBuilder.Entity<User>().HasData(user);

            var roles = new[]
            {
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
            };
            modelBuilder.Entity<Role>().HasData(roles);

            var userRoles = new[]
            {
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
            };
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);

            var incomes = new Income[6];
            for(int i = 0; i < 6; i++)
            {
                incomes[i] = new Income
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryId = "fb9d333d-5e85-484b-bb35-ef307fd06379",
                    Date = DateTime.Now.AddDays(-5).AddMonths(-i),
                    UserId = "830f0d08-6a56-4a57-83c9-329c094f184b",
                    Value = 2345.67,
                    Description = "Spadek po cioci"
                };
            }
            modelBuilder.Entity<Income>().HasData(incomes);
        }
    }
}