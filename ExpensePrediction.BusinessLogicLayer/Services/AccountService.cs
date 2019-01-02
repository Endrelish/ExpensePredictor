using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects.User;
using ExpensePrediction.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;

        //TODO Delet dis
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;
        private readonly IApplicationRepository<User> repo;

        public AccountService(UserManager<User> userManager, IMapper mapper, IPasswordHasher<User> passwordHasher,
            IApplicationRepository<User> repo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            this.repo = repo;
        }

        public async Task ChangePasswordAsync(PasswordChangeDto passwordChangeDto, string userId)
        {
            if (passwordChangeDto.NewPassword != passwordChangeDto.NewPasswordRepeated)
            {
                throw new AccountException("Passwords don't match", 400);
            }

            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, passwordChangeDto.CurrentPassword,
                passwordChangeDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new AccountException("Unknown error", 500);
            }
        }

        public async Task<UserDataDto> EditUserDataAsync(UserEditDto userEditDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.FirstName = userEditDto.FirstName;
            user.LastName = userEditDto.LastName;
            user.PhoneNumber = userEditDto.PhoneNumber;

            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserDataDto>(user);
        }

        public async Task<UserDataDto> GetUserDataAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<UserDataDto>(user);
        }

        //TODO Delet dis
        public async Task ResetPassAsync(string userId)
        {
            var user = (await repo.FindByConditionAync(u => u.UserName == userId)).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, user.UserName + "pass");
            await repo.SaveAsync();
        }
    }
}