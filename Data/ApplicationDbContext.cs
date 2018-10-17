using AuthWebApi.Data.Entities.Expenses;
using AuthWebApi.Data.Entities.Users;
using AuthWebApi.Data.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ActivationToken> ActivationTokens { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                    FirstName = "test",
                    LastName = "test",
                    UserName = "test",
                    NormalizedUserName = "TEST",
                    Email = "a@a.a",
                    NormalizedEmail = "A@A.A",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEODZFtx31yVlQlAo6GcTs2dIyi/Dcch0/uqv27PvT/xXzy5+JAZEMVS5SvM13yrMdQ==",
                    PhoneNumber = "123456789"
                },
                new User
                {
                    Id = "73dcc714-8fbc-41ac-a6af-756986ade684",
                    FirstName = "test",
                    LastName = "test",
                    UserName = "test2",
                    NormalizedUserName = "TEST2",
                    Email = "a@a.a",
                    NormalizedEmail = "A@A.A",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEODZFtx31yVlQlAo6GcTs2dIyi/Dcch0/uqv27PvT/xXzy5+JAZEMVS5SvM13yrMdQ==",
                    PhoneNumber = "123456780"
                }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "fb435b98-bd28-4a20-ab4a-62b124d9841b",
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "fb467b98-bd28-6720-ab4a-645124d9834b",
                    Name = "user",
                    NormalizedName = "USER"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                    RoleId = "fb435b98-bd28-4a20-ab4a-62b124d9841b"
                },
                new IdentityUserRole<string>
                {
                    UserId = "fa435b98-bd28-4a20-8b4a-62b124d9841b",
                    RoleId = "fb467b98-bd28-6720-ab4a-645124d9834b"
                },
                new IdentityUserRole<string>
                {
                    UserId = "73dcc714-8fbc-41ac-a6af-756986ade684",
                    RoleId = "fb467b98-bd28-6720-ab4a-645124d9834b"
                }
            );
        }
    }
}
