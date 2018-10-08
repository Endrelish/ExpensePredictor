using System;
using AuthWebApi.Data.Expenses.Entities;
using AuthWebApi.Data.Users.Entities;

namespace AuthWebApi.Data.Entities.Expenses
{
    public class Transaction<T> where T : Category
    {
        public string Id { get; set; }
        public User User { get; set; }
        //public string UserId { get; set; }
        public double Value { get; set; }
        public T Category { get; set; }
        //public string CategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}