using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthWebApi.Controllers
{
    [Route("/api/[controller]")]
    public class AuthController : Controller
    {
        private DatabaseContext _context;
        private IPasswordHasher<User> _hasher;

        public AuthController(DatabaseContext context, IPasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }
        [HttpGet("haha")]
        [Authorize]
        public IActionResult Haha()
        {
            return Ok("haha");
        }
        [HttpPost("login")]
        public IActionResult CreateToken([FromBody]CredentialModel credentials)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName.Equals(credentials.Username, StringComparison.OrdinalIgnoreCase));
                // user.PasswordHash = _hasher.HashPassword(user, credentials.Password);
                // _context.SaveChanges();
                if (_hasher.VerifyHashedPassword(user, user.PasswordHash, credentials.Password) == PasswordVerificationResult.Success)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mySuperSecretKey"));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddYears(1),
                    signingCredentials: creds);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return StatusCode(400);
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
        }
    }

    public class CredentialModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}