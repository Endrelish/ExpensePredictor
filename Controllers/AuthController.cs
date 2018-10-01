using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthWebApi.Data;
using AuthWebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthWebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, IPasswordHasher<User> hasher, IConfiguration configuration, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _hasher = hasher;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(400, "ERROR");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Success)
            {
                var token = await GenerateJwtToken(user);
                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok("You are in");
        }

        [HttpGet]
        [Authorize("admin")]
        public IActionResult Admin()
        {
            return Ok("You are an admin");
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = roles.Select(r => new Claim("identityRoles", r));
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            claims = claims.Union(rolesClaims).ToList();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}