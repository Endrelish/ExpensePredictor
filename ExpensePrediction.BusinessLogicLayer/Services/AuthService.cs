using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects.User;
using ExpensePrediction.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper,
            UserManager<User> userManager,
            IConfiguration configuration,
            IPasswordHasher<User> hasher)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _hasher = hasher;
        }

        public async Task<TokenDto> GetTokenAsync(LoginDto loginData)
        {
            var user = await _userManager.FindByNameAsync(loginData.Username);
            if (user != null)
            {
                var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password);

                if ((await _userManager.GetRolesAsync(user)).Any())
                {
                    if (result == PasswordVerificationResult.Success)
                    {
                        return await GenerateJwtTokenAsync(user);
                    }
                }
            }

            throw new AuthException("Username or password incorrect", 400);
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto registerData)
        {
            var user = _mapper.Map<User>(registerData);

            var userCreateResult = await _userManager.CreateAsync(user, registerData.Password);
            var userRolesResult = await SetDefaultRolesAsync(user);

            if (userCreateResult.Succeeded && userRolesResult)
            {
                var token = await GenerateJwtTokenAsync(user);
                return token;
            }

            throw new AuthException("Registration not successful", 400);
        }

        private async Task<TokenDto> GenerateJwtTokenAsync(User user)
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
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenDto
            {
                UserId = user.Id,
                ExpireDate = expires,
                Token = tokenString
            };
        }

        private async Task<bool> SetDefaultRolesAsync(User user)
        {
            var roleNames = _configuration.GetSection("DefaultRoles").Get<List<string>>();

            var result = await _userManager.AddToRolesAsync(user, roleNames);
            return result.Succeeded;
        }
    }
}