using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IAccountService
    {
        Task<UserDataDto> GetUserDataAsync(string userId);
        Task ChangePasswordAsync(PasswordChangeDto passwordChangeDto, string userId);
        Task<UserDataDto> EditUserDataAsync(UserEditDto userEditDto, string userId);
        Task ResetPassAsync(string userId);
    }
}