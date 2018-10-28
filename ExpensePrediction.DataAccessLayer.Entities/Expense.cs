namespace ExpensePrediction.DataAccessLayer.Entities
{
    public class Expense : Transaction<ExpenseCategory>
    {
        public virtual Expense LinkedExpense { get; set; }
    }
}