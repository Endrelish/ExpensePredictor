using AuthWebApi.Dto;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IApplicationRepository<IdentityRole> _roleRepository;
        private readonly IApplicationRepository<IdentityUserRole<string>> _userRoleRepository;

        public AuthService(IMapper mapper,
            UserManager<User> userManager,
            IConfiguration configuration,
            IPasswordHasher<User> hasher,
            IApplicationRepository<IdentityUserRole<string>> userRoleRepository,
            IApplicationRepository<IdentityRole> roleRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _hasher = hasher;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<string> GetToken(LoginDto loginData)
        {
            var user = await _userManager.FindByNameAsync(loginData.Username);
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return await GenerateJwtToken(user);
            }

            throw new Exception(); //TODO custom exceptions
        }

        public async Task<string> Register(RegisterDto registerData)
        {
            var user = _mapper.Map<User>(registerData);

            var userCreateResult = await _userManager.CreateAsync(user, registerData.Password);
            var userRolesResult = await SetDefaultRoles(user);

            if (userCreateResult.Succeeded && userRolesResult)
            {
                var token = await GenerateJwtToken(user);
                return token;
            }

            //else userCreateResult.Errors => exceptions

            throw new Exception(); //TODO custom exceptions
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

        private async Task<bool> SetDefaultRoles(User user)
        {
            var roleNames = _configuration.GetSection("DefaultRoles").Get<List<string>>();
            var roles = await _roleRepository.FindByConditionAync(r => roleNames.Contains(r.Name, StringComparer.OrdinalIgnoreCase));

            var tasks = new List<Task>();
            foreach(var role in roles)
            {
                var userRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                tasks.Add(_userRoleRepository.CreateAsync(userRole));
            }

            await Task.WhenAll(tasks);

            return await _userRoleRepository.SaveAsync() > 0;
        }
    }
}