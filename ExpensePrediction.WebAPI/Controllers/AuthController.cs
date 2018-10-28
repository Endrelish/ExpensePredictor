using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthWebApi.Dto;
using AutoMapper;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _hasher;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthController(UserManager<User> userManager, IPasswordHasher<User> hasher, IConfiguration configuration,
            SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _hasher = hasher;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new user with specified data.
        /// </summary>
        /// <param name="registrationData">The registration data.</param>
        /// <returns></returns>
        [HttpPost("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registrationData)
        {
            var user = _mapper.Map<User>(registrationData);

            var result = await _userManager.CreateAsync(user, registrationData.Password);

            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                return Ok(new { Token = token});
            }

            return StatusCode(400, "ERROR"); //TODO custom error codes
        }

        /// <summary>
        /// Gets the token for specified user.
        /// </summary>
        /// <param name="loginData">The login data.</param>
        /// <returns>The token.</returns>
        [HttpPost("get-token")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetToken([FromBody] LoginDto loginData)
        {
            var user = await _userManager.FindByNameAsync(loginData.Username);
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password);

            if (result == PasswordVerificationResult.Success)
            {
                var token = await GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = roles.Select(r => new Claim("identityRoles", r));
            var claims = new List<Claim>
            {
                new Claim("username", user.UserName),
                // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
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