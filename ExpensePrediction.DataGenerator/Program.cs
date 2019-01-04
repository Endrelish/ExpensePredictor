using ExpensePrediction.DataAccessLayer;
using ExpensePrediction.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensePrediction.DataGenerator
{
    class Program
    {
        private static readonly ApplicationDbContext _context;

        static Program()
        {
            _context = GetContext();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Clearing expenses and incomes...");
            ClearExpensesAndIncomes().Wait();

        }

        private async static Task ClearExpensesAndIncomes()
        {
            _context.RemoveRange(await _context.Incomes.ToListAsync());
            _context.RemoveRange(await _context.Expenses.ToListAsync());

            await _context.SaveChangesAsync();
        }

        private async static Task InsertBasicIncomes()
        {
            foreach(var user in _context.Users)
            {
                var months = 24;
                double lowerLimit = 2000.0d, upperLimit = 100000.0d, tolerance = 0.1d;
                var rnd = new Random();
                var income = (double)rnd.Next((int)lowerLimit * 100, (int)upperLimit * 100);
                income /= 100.0d;
                await InsertUserIncomes(user, income, tolerance, months);
            }
        }

        private async static Task InsertUserIncomes(User user, double income, double tolerance, int months)
        {
            var rnd = new Random();
            var category = await _context.IncomeCategories.Select(c => c.Id).FirstAsync();
            for(int i = 0; i < months; i++)
            {
                await _context.AddAsync(new Income
                {
                    CategoryId = category,
                    Date = DateTime.Now.AddMonths(-i),
                    Description = "Income",
                    UserId = user.Id,
                    Value = income + (rnd.NextDouble() * 2 - 1) * tolerance * income
                });
            }
        }

        private async static Task InsertUserExpensesAsync(User user)
        {

        }

        private static ApplicationDbContext GetContext()
        {
            var builder = new DbContextOptionsBuilder();
            var connectionString = @"Server = tcp:127.0.0.1,1435; Initial Catalog = expenses - prediction; Persist Security Info = False; User ID = user; Password = userpass; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = True; Connection Timeout = 30;";
            //var connectionString = @"Server=tcp:expenses-prediction.database.windows.net,1433;Initial Catalog=expenses-prediction;Persist Security Info=False;User ID=ppurgat;Password=zL7@5B*@!H;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
