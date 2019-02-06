using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.Frontend.Service
{
    public class ExpenseService
    {
        public async Task<List<ExpenseDto>> GetExpenses(DateTime dateFrom, DateTime dateTo)
        {
            return await RestService.GetAsync<List<ExpenseDto>>(Constants.GetExpensesUri(dateFrom, dateTo));
        }

        public async Task<IncomeDto> AddExpenseAsync(TransactionDto dto)
        {
            return await RestService.PostAsync<IncomeDto>(Constants.AddExpenseUri, dto);
        }

        public async Task DeleteExpenseAsync(string dtoId)
        {
            await RestService.DeleteAsync(Constants.DeleteExpenseUri + dtoId);
        }

        public async Task EditExpense(TransactionDto dto)
        {
            await RestService.PostAsync<object>(Constants.EditExpenseUri, dto);
        }
    }
}
