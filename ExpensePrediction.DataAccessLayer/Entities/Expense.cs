namespace ExpensePrediction.DataAccessLayer.Entities.Expenses
{
    public class Expense : Transaction<ExpenseCategory>
    {
        public Expense LinkedExpense { get; set; }
    }
}