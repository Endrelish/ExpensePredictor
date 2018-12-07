using System;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;
using Xamarin.Essentials;

namespace ExpensePrediction.Frontend.Service
{
    public class AuthService
    {
        public async Task LoginAsync(LoginDto loginDto, Action navigateAsync)
        {
            var token = await RestService.PostAsync<TokenDto>(Constants.GetTokenUri, loginDto, false);
            var a = token.UserId;
            var b = token.ExpireDate;
            var c = token.Token;
            await SetTokenAsync(token);
            navigateAsync();
        }

        public async Task RegisterAsync(RegisterDto registerDto, Action navigateAsync)
        {
            var token = await RestService.PostAsync<TokenDto>(Constants.RegisterUri, registerDto, false);
            await SetTokenAsync(token);
            navigateAsync();
        }

        private async Task SetTokenAsync(TokenDto token)
        {

            await SecureStorage.SetAsync(Constants.Token, token.Token);
            await SecureStorage.SetAsync(Constants.UserId, token.UserId);
            //TODO set some task to obtain a new token when this one expires if credentials are remembered

            App.IsUserLoggedIn = true;
        }

        public void Logout(Action navigateAsync)
        {
            SecureStorage.Remove(Constants.Token);
            SecureStorage.Remove(Constants.UserId);
            App.IsUserLoggedIn = false;
            navigateAsync();
        }
    }
}