using System;

namespace ExpensePrediction.DataTransferObjects
{
    public class ExpenseDto : TransactionDto
    {
        public string LinkedExpenseId { get; set; }
    }
}