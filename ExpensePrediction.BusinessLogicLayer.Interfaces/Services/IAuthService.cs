using AuthWebApi.Dto;
using ExpensePrediction.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerData);
        Task<string> GetToken(LoginDto loginData);
    }
}
