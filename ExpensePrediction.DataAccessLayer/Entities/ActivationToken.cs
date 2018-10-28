using System;

namespace ExpensePrediction.DataAccessLayer.Entities.Users
{
    public class ActivationToken
    {
        public string Id { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}