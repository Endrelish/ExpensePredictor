using AuthWebApi.Dto;
using ExpensePrediction.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ExpensePrediction.Frontend.Service
{
    public class AuthService
    {
        private readonly RestService _restService;
        public AuthService()
        {
            _restService = new RestService();
        }

        public async Task Login(LoginDto loginDto)
        {
            var token = await _restService.PostAsync<TokenDto>(Constants.GetTokenUri, loginDto, false);
            try
            {
                await SecureStorage.SetAsync(Constants.Token, token.Token);
                //TODO set some task to obtain a new token when this one expires
            }
            catch (Exception e)
            {
                //TODO don't know what
                throw;
            }

            App.IsUserLoggedIn = true;
        }
    }
}
