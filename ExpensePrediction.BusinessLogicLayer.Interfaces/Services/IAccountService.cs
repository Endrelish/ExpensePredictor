using ExpensePrediction.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IAccountService
    {
        Task<UserDataDto> GetUserData(string userId);
        Task ChangePassword(PasswordChangeDto passwordChangeDto, string userId);
        Task<UserDataDto> EditUserData(UserEditDto userEditDto, string userId);
    }
}
