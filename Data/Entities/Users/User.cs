using Microsoft.AspNetCore.Identity;

namespace AuthWebApi.Data.Users.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}