using System;

namespace ExpensePrediction.DataTransferObjects.Expense
{
    public class ExpenseDto
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string CategoryId { get; set; }
        public string LinkedExpenseId { get; set; }
    }
}