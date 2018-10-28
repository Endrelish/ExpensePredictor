using Microsoft.AspNetCore.Identity;

namespace ExpensePrediction.DataAccessLayer.Entities
{
    public class User : IdentityUser, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}