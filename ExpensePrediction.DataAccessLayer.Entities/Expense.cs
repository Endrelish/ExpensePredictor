namespace ExpensePrediction.DataAccessLayer.Entities
{
    public class Expense : Transaction<ExpenseCategory>
    {
        public string LinkedExpenseId { get; set; }
        public virtual Expense LinkedExpense { get; set; }
    }
}