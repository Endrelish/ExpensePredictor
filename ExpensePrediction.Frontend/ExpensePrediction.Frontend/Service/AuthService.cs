using System;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;
using Xamarin.Essentials;

namespace ExpensePrediction.Frontend.Service
{
    public class AuthService
    {
        private readonly RestService _restService;

        public AuthService()
        {
            _restService = new RestService();
        }

        public async Task LoginAsync(LoginDto loginDto, Action navigateAsync)
        {
            var token = await _restService.PostAsync<TokenDto>(Constants.GetTokenUri, loginDto, false);
            await SetTokenAsync(token);
            navigateAsync();
        }

        public async Task RegisterAsync(RegisterDto registerDto, Action navigateAsync)
        {
            var token = await _restService.PostAsync<TokenDto>(Constants.RegisterUri, registerDto, false);
            await SetTokenAsync(token);
            navigateAsync();
        }

        private async Task SetTokenAsync(TokenDto token)
        {
            await SecureStorage.SetAsync(Constants.Token, token.Token);
            //TODO set some task to obtain a new token when this one expires if credentials are remembered

            App.IsUserLoggedIn = true;
        }

        public void Logout(Action navigateAsync)
        {
            SecureStorage.Remove(Constants.Token);
            App.IsUserLoggedIn = false;
            navigateAsync();
        }
    }
}