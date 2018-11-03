using System.Threading.Tasks;
using AuthWebApi.Dto;
using ExpensePrediction.DataTransferObjects.User;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerData);
        Task<string> GetToken(LoginDto loginData);
    }
}