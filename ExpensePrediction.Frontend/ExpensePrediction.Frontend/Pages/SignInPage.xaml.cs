using AuthWebApi.Dto;
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
		public SignInPage ()
		{
			InitializeComponent ();
		}

        private void SignInClicked(object sender, EventArgs e)
        {
            var loginDto = new LoginDto
            {
                Username = Username.Text,
                Password = Password.Text
            };
        }
    }
}