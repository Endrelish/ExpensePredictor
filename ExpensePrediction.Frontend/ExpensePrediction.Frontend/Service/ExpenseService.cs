﻿using ExpensePrediction.DataTransferObjects;
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
    }
}