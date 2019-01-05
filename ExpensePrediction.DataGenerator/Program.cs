using System;
using System.Linq;
using System.Threading.Tasks;
using ExpensePrediction.DataAccessLayer;
using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensePrediction.DataGenerator
{
    internal class Program
    {
        const int Months = 24;
        private const int Users = 50;
        private const double CoefficientsTolerance = 0.2d;
        private const double MainExpenseCoefficient = 0.75d;
        private const double FirstMonthCoefficient = 0.2d;
        private const double SecondMonthCoefficient = 0.175d;
        private const double ThirdMonthCoefficient = 0.09835d;
        private static readonly Random Rnd = new Random();
        private static readonly ApplicationDbContext Context;

        static Program()
        {
            Context = GetContext(false);
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Clearing expenses and incomes...");
            ClearExpensesAndIncomesAsync().Wait();
            Console.WriteLine("Inserting users...");
            CreateUsersAsync().Wait();
            Console.WriteLine("Inserting regular expenses and incomes...");
            InsertBasicIncomesAndExpensesAsync().Wait();
            Console.WriteLine("Inserting main and linked expenses...");
        }

        private static async Task InsertMainAndLinkedExpenses()
        {
            foreach (var user in Context.Users)
            {
                var month = Rnd.Next(4, Months);
            }
        }

        private static double GetToleranceModifier()
        {
            return 1.0d - CoefficientsTolerance + 2.0d * CoefficientsTolerance * Rnd.NextDouble();
        }

        private static async Task ClearExpensesAndIncomesAsync()
        {
            Context.RemoveRange(await Context.Incomes.ToListAsync());
            Context.RemoveRange(await Context.Expenses.ToListAsync());

            await Context.SaveChangesAsync();
        }

        private static async Task CreateUsersAsync()
        {
            for (var i = 0; i < Users; i++)
            {
                var guid = Guid.NewGuid().ToString();
                await Context.AddAsync(new User
                {
                    UserName = guid,
                    NormalizedUserName = guid.ToUpperInvariant(),
                    Email = guid,
                    NormalizedEmail = guid.ToUpper(),
                    EmailConfirmed = false,
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEAqh0aPHTDpA+v4DWceLGwWnCl5v73f/qrFIXkktV7VRo4ocioRqt2kneYJ50JPjxg==",
                    SecurityStamp = "BMQFGCNZOJDFTYMB6XFUCQY4FFJOPGHJ",
                    ConcurrencyStamp = "8bc49ec0-656f-4618-907a-a45e8cee2eaa",
                    PhoneNumber = "123456780",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                });
            }
        }

        private static async Task InsertBasicIncomesAndExpensesAsync()
        {
            foreach (var user in Context.Users)
            {
                const double lowerLimit = 2000.0d, upperLimit = 100000.0d;
                const double tolerance = 0.1d;
                var income = (double) Rnd.Next((int) lowerLimit * 100, (int) upperLimit * 100);
                income /= 100.0d;
                var expense = income * 0.8d;
                await InsertUserIncomesAsync(user, income, tolerance, Months);
                await InsertUserExpensesAsync(user, expense, tolerance, Months);
            }
        }

        private static async Task InsertUserIncomesAsync(User user, double income, double tolerance, int months)
        {
            var category = await Context.IncomeCategories.Select(c => c.Id).FirstAsync();
            for (var i = 0; i < months; i++)
            {
                await Context.AddAsync(new Income
                {
                    CategoryId = category,
                    Date = DateTime.Now.AddMonths(-i),
                    Description = "Income",
                    UserId = user.Id,
                    Value = income + (Rnd.NextDouble() * 2 - 1) * tolerance * income
                });
            }
        }

        private static async Task InsertUserExpensesAsync(User user, double expense, double tolerance, int months)
        {
            var category = await Context.ExpenseCategories.Select(c => c.Id).FirstAsync();
            for (var i = 0; i < months; i++)
            {
                await Context.AddAsync(new Expense
                {
                    CategoryId = category,
                    Date = DateTime.Now.AddMonths(-i),
                    Description = "Expense",
                    UserId = user.Id,
                    Value = expense + (Rnd.NextDouble() * 2 - 1) * tolerance * expense
                });
            }
        }

        private static ApplicationDbContext GetContext(bool useLocal)
        {
            var builder = new DbContextOptionsBuilder();
            const string connectionStringLocal = @"Server=tcp:127.0.0.1,1435;Initial Catalog=expenses-prediction;
                Persist Security Info=False;User ID=user;Password=userpass;MultipleActiveResultSets=False;
                Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
            const string connectionStringRemote = @"Server=tcp:expenses-prediction.database.windows.net,1433;
                Initial Catalog=expenses-prediction;Persist Security Info=False;
                User ID=ppurgat;Password=zL7@5B*@!H;MultipleActiveResultSets=False;
                Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            builder.UseSqlServer(useLocal ? connectionStringLocal : connectionStringRemote);
            return new ApplicationDbContext(builder.Options);
        }
    }
}