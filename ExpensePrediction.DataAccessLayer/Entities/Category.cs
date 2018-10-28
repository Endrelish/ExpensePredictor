namespace ExpensePrediction.DataAccessLayer.Entities.Expenses
{
    public class Category : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}