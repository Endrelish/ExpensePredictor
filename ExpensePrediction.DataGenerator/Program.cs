using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ExpensePrediction.DataAccessLayer;
using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensePrediction.DataGenerator
{
    internal class Program
    {
        const int Months = 24;
        private const int Users = 4;
        private const double CoefficientsTolerance = 0.2d;
        private const double FirstMonthExpenseCoefficient = 0.2d;
        private const double SecondMonthExpenseCoefficient = 0.175d;
        private const double ThirdMonthExpenseCoefficient = 0.09835d;
        private const double FirstMonthIncomeCoefficient = 0.1d;
        private const double SecondMonthIncomeCoefficient = 0.0945d;
        private const double ThirdMonthIncomeCoefficient = 0.02233d;
        private const double FirstMonthRegularExpenseCoefficient = 0.01d;
        private const double SecondMonthRegularExpenseCoefficient = 0.009d;
        private const double ThirdMonthRegularExpenseCoefficient = 0.0065d;
        private const double LowerIncomeLimit = 2000.0d;
        private const double UpperIncomeLimit = 100000.0d;
        private static readonly Random Rnd = new Random();
        private static readonly ApplicationDbContext Context = GetContext(false);
        private static readonly string ExpenseCategoryId = Context.ExpenseCategories.Select(c => c.Id).First();
        private static int _changedRecords = 0;
        
        private static void Main(string[] args)
        {
            Console.WriteLine("Clearing expenses and incomes...");
            ClearExpensesAndIncomesAsync().Wait();
            Console.WriteLine("Done.");
            Console.WriteLine("Inserting users...");
            CreateUsersAsync().Wait();
            Console.WriteLine("Done.");
            Console.WriteLine("Inserting regular expenses and incomes...");
            InsertBasicIncomesAndExpensesAsync().Wait();
            Console.WriteLine("Done.");
            Console.WriteLine("Inserting main and linked expenses...");
            InsertMainAndLinkedExpenses().Wait();
            Console.WriteLine("Done.");
            Console.WriteLine("{0} records inserted.", _changedRecords);
            Console.ReadKey();
        }

        private static async Task InsertMainAndLinkedExpenses()
        {
            var date = DateTime.Now;
            foreach (var user in Context.Users)
            {
                var regularExpense = Context.Expenses.Where(e => e.UserId == user.Id &&
                                                                 e.Date.Year == date.Year &&
                                                                 e.Date.Month == date.Month &&
                                                                 e.CategoryId == ExpenseCategoryId)
                    .Sum(e => e.Value);

                Expression<Func<Income, bool>> userIncome = i =>
                    i.UserId == user.Id &&
                    i.Date.Month == DateTime.Now.Month &&
                    i.Date.Year == DateTime.Now.Year;
                var income = Context.Incomes.First(userIncome);

                var month = Rnd.Next(4, Months);
                var mainExpense = new Expense
                {
                    CategoryId = ExpenseCategoryId,
                    Date = DateTime.Now.AddMonths(-month),
                    Description = "MainExpense",
                    UserId = user.Id,
                    Value = Rnd.Next((int) LowerIncomeLimit * 500, (int) UpperIncomeLimit * 500) / 100.0d,
                    Main = true
                };
                month--;
                var firstMonthExpense = new Expense
                {
                    CategoryId = ExpenseCategoryId,
                    Date = DateTime.Now.AddMonths(-month),
                    Description = "FirstMonthExpense",
                    UserId = user.Id,
                    Value = GetToleranceModifier() * FirstMonthExpenseCoefficient * mainExpense.Value
                            + GetToleranceModifier() * FirstMonthIncomeCoefficient * income.Value
                            + GetToleranceModifier() * FirstMonthRegularExpenseCoefficient * regularExpense
                };
                month--;
                var secondMonthExpense = new Expense
                {
                    CategoryId = ExpenseCategoryId,
                    Date = DateTime.Now.AddMonths(-month),
                    Description = "SecondMonthExpense",
                    UserId = user.Id,
                    Value = GetToleranceModifier() * SecondMonthExpenseCoefficient * mainExpense.Value
                            + GetToleranceModifier() * SecondMonthIncomeCoefficient * income.Value
                            + GetToleranceModifier() * SecondMonthRegularExpenseCoefficient * regularExpense
                };
                month--;
                var thirdMonthExpense = new Expense
                {
                    CategoryId = ExpenseCategoryId,
                    Date = DateTime.Now.AddMonths(-month),
                    Description = "ThirdMonthExpense",
                    UserId = user.Id,
                    Value = GetToleranceModifier() * ThirdMonthExpenseCoefficient * mainExpense.Value
                            + GetToleranceModifier() * ThirdMonthIncomeCoefficient * income.Value
                            + GetToleranceModifier() * ThirdMonthRegularExpenseCoefficient * regularExpense
                };

                await Context.AddAsync(mainExpense);
                await Context.AddAsync(firstMonthExpense);
                await Context.AddAsync(secondMonthExpense);
                await Context.AddAsync(thirdMonthExpense);
            }

            _changedRecords += await Context.SaveChangesAsync();
        }

        private static double GetToleranceModifier()
        {
            return 1.0d - CoefficientsTolerance + 2.0d * CoefficientsTolerance * Rnd.NextDouble();
        }

        private static async Task ClearExpensesAndIncomesAsync()
        {
            Context.RemoveRange(await Context.Incomes.ToListAsync());
            Context.RemoveRange(await Context.Expenses.ToListAsync());

            _changedRecords += await Context.SaveChangesAsync();
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

            _changedRecords += await Context.SaveChangesAsync();
        }

        private static async Task InsertBasicIncomesAndExpensesAsync()
        {
            foreach (var user in Context.Users)
            {
                const double tolerance = 0.1d;
                var income = (double) Rnd.Next((int) LowerIncomeLimit * 100, (int) UpperIncomeLimit * 100);
                income /= 100.0d;
                var expense = income * 0.8d;
                await InsertUserIncomesAsync(user, income, tolerance, Months);
                await InsertUserExpensesAsync(user, expense, tolerance, Months);
            }

            _changedRecords += await Context.SaveChangesAsync();
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
            for (var i = 0; i < months; i++)
            {
                await Context.AddAsync(new Expense
                {
                    CategoryId = ExpenseCategoryId,
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
            const string connectionStringLocal = @"Server=DESKTOP-6M4TN9H\SQLEXPRESS;
                Initial Catalog=expenses-prediction;Persist Security Info=False;User ID=user;
                Password=userpass;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;
                Connection Timeout=30;";
            const string connectionStringRemote = @"Server=tcp:expenses-prediction.database.windows.net,1433;
                Initial Catalog=expenses-prediction;Persist Security Info=False;
                User ID=ppurgat;Password=zL7@5B*@!H;MultipleActiveResultSets=False;
                Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            builder.UseSqlServer(useLocal ? connectionStringLocal : connectionStringRemote);
            return new ApplicationDbContext(builder.Options);
        }
    }
}