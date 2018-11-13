using AuthWebApi.Dto;
using ExpensePrediction.Frontend.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPage : ContentPage
	{
        private readonly AuthService _authService;
		public SignInPage ()
		{
			InitializeComponent ();

            _authService = new AuthService();
		}

        private async Task SignInClicked(object sender, EventArgs e)
        {
            var loginDto = new LoginDto
            {
                Username = Username.Text,
                Password = Password.Text
            };

            await _authService.Login(loginDto);
        }
    }
}