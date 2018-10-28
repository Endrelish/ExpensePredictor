using System;

namespace ExpensePrediction.DataAccessLayer.Entities.Users
{
    public class ActivationToken : IEntity
    {
        public string Id { get; set; }
        public virtual User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}