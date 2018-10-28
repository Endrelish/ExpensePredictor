using Microsoft.AspNetCore.Identity;

namespace ExpensePrediction.DataAccessLayer.Entities.Users
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}