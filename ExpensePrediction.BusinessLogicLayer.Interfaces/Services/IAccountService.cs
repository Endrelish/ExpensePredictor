using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IAccountService
    {
        Task<UserDataDto> GetUserData(string userId);
        Task ChangePassword(PasswordChangeDto passwordChangeDto, string userId);
        Task<UserDataDto> EditUserData(UserEditDto userEditDto, string userId);
    }
}