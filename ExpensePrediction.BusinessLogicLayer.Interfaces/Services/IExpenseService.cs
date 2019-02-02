﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<ExpenseDto> GetExpenseAsync(string expenseId, string userId);
        Task<IEnumerable<ExpenseDto>> GetExpensesAsync(string userId, DateTime from, DateTime to);
        Task<ExpenseDto> AddExpenseAsync(ExpenseDto expense, string userId);
        Task<ExpenseDto> EditExpenseAsync(ExpenseDto expense, string userId);
    }
}