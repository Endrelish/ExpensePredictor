using System;
using ExpensePrediction.DataTransferObjects.User;
using ExpensePrediction.Exceptions;
using ExpensePrediction.Frontend.Service;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        private readonly AuthService _authService;

        public SignInPage()
        {
            InitializeComponent();

            _authService = new AuthService();
        }

        private async void SignInClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var loginDto = new LoginDto
            {
                Username = Username.Text,
                Password = Password.Text
            };

            try
            {
                void Navigate()
                {
                    Application.Current.MainPage = new StartPage();
                }

                await _authService.LoginAsync(loginDto, Navigate);
            }
            catch (RestException re)
            {
                await DisplayAlert("Error", re.Message, "");
            }
            finally
            {
                await ActivityIndicatorPage.ToggleIndicator(false);
            }
        }
    }
}