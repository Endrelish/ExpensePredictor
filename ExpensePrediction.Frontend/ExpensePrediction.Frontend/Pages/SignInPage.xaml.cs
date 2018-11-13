using AuthWebApi.Dto;
using ExpensePrediction.Frontend.Service;
using System;

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
            var loginDto = new LoginDto
            {
                Username = Username.Text,
                Password = Password.Text
            };

            try
            {
                await _authService.Login(loginDto);

                Navigation.InsertPageBefore(new StartPage(), this);
                await Navigation.PopAsync();
            }
            catch (Exception)
            {
                //TODO display a message i guess
            }
        }
    }
}