using System;
using ExpensePrediction.Frontend.Pages;
using Xamarin.Forms;

namespace ExpensePrediction.Frontend
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignIn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
        }

        private async void SignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}