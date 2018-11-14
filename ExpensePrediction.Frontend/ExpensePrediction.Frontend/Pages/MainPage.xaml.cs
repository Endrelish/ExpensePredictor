using ExpensePrediction.Frontend.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void SignUp(object sender, EventArgs e)
        {

        }
    }
}
