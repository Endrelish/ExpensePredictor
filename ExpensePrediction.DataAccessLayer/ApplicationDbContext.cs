using ExpensePrediction.DataAccessLayer.Entities.Expenses;
using ExpensePrediction.DataAccessLayer.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensePrediction.DataAccessLayer
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

            modelBuilder.Entity<ExpenseCategory>().HasData(
                new ExpenseCategory()
                {
                    Id = "27a62b11-7765-473d-a1b6-7ecda9915dd5",
                    Name = "¯arcie"
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
                    Id = "d95be18c-eb08-40fb-a082-e4073546c7d4",
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
                    UserId = "d95be18c-eb08-40fb-a082-e4073546c7d4",
                    RoleId = "fb435b98-bd28-4a20-ab4a-62b124d9841b"
                },
                new IdentityUserRole<string>
                {
                    UserId = "d95be18c-eb08-40fb-a082-e4073546c7d4",
                    RoleId = "fb467b98-bd28-6720-ab4a-645124d9834b"
                }
            );
        }
    }
}
