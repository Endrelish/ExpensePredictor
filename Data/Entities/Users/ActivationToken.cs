using System;
using AuthWebApi.Data.Users.Entities;

namespace AuthWebApi.Data.Entities.Users
{
    public class ActivationToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}