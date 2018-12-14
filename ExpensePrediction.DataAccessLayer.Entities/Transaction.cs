using System;

namespace ExpensePrediction.DataAccessLayer.Entities
{
    public class Transaction<T> : IEntity where T : Category
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public double Value { get; set; }
        public string CategoryId { get; set; }
        public virtual T Category { get; set; }
        public DateTime Date { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
    }
}