using System;

namespace ExpensePrediction.DataTransferObjects
{
    public class ExpenseDto
    {
        public string Id { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string CategoryId { get; set; }
        public string LinkedExpenseId { get; set; }
    }
}