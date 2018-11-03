using System;

namespace ExpensePrediction.DataAccessLayer.Entities
{
    public class ActivationToken : IEntity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Id { get; set; }
    }
}