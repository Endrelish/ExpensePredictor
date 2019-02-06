using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;

namespace ExpensePrediction.Frontend.Service
{
    public class AccountService
    {
        public async Task ChangePasswordAsync(PasswordChangeDto dto)
        {
            await RestService.PostAsync<object>(Constants.ChangePasswordUri, dto);
        }
    }
}
