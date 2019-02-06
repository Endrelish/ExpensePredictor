using ExpensePrediction.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ExpensePrediction.Frontend.Service
{
    class UserService
    {
        private static UserDataDto _userData;
        private static string _userId = string.Empty;
        
        public async Task<UserDataDto> GetUserDetailsAsync()
        {
            var userId = await SecureStorage.GetAsync(Constants.UserId);
            if (!_userId.Equals(userId, StringComparison.InvariantCultureIgnoreCase))
                await SetUserDataAsync();
            return _userData;
        }

        private async Task SetUserDataAsync()
        {
            _userData = await RestService.GetAsync<UserDataDto>(Constants.GetUserUri);
        }

        public async Task EditUser(UserEditDto dto)
        {
            await RestService.PostAsync<UserEditDto>(Constants.EditUserUri, dto);
        }
    }
}
