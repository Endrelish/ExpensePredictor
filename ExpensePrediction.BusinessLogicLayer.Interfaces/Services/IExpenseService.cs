using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<ExpenseDto> GetExpense(string expenseId, string userId);
        Task<IEnumerable<ExpenseDto>> GetExpenses(string userId);
        Task<ExpenseDto> AddExpense(ExpenseDto expense, string userId);
        Task<ExpenseDto> EditExpense(ExpenseDto expense, string userId);
        Task<IEnumerable<ExpenseDto>> GetLinkedExpenses(string expenseId, string userId);

    }
}
