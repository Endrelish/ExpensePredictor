using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterAsync(RegisterDto registerData);
        Task<TokenDto> GetTokenAsync(LoginDto loginData);
    }
}