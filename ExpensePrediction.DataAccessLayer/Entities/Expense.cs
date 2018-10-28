namespace ExpensePrediction.DataAccessLayer.Entities.Expenses
{
    public class Expense : Transaction<ExpenseCategory>
    {
        public virtual Expense LinkedExpense { get; set; }
    }
}