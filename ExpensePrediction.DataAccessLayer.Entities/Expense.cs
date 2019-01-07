namespace ExpensePrediction.DataAccessLayer.Entities
{
    public class Expense : Transaction<ExpenseCategory>
    {
        public bool Main { get; set; }
    }
}
