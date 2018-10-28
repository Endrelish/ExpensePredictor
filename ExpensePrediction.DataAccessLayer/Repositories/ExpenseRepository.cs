using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ExpensePrediction.DataAccessLayer.Entities;

namespace ExpensePrediction.DataAccessLayer.Repositories
{
    public class ExpenseRepository : ApplicationRepository<Expense>
    {
        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override async Task<Expense> FindByIdAsync(string id)
        {
            return await _dbContext.Set<Expense>().Include(e => e.LinkedExpense).Include(e => e.Category)
                .Include(e => e.User).SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
