using AuthWebApi.Dto;
using ExpensePrediction.Frontend.Service;
using System;
using System.Threading.Tasks;
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
                Func<Task> navigateAsync = async () =>
                {
                    App.Current.MainPage = new StartPage();
                    //await App.Current.MainPage.Navigation.PopAsync();
                    //Navigation.InsertPageBefore(new StartPage(), this);
                    //await Navigation.PopAsync();
                };
                await _authService.LoginAsync(loginDto, navigateAsync);
            }
            catch (Exception)
            {
                //TODO display a message i guess
            }
        }
    }
}