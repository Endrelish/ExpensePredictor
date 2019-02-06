using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.User;
using ExpensePrediction.Exceptions;
using ExpensePrediction.Frontend.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PasswordChangePage : ContentPage
	{
	    private readonly AccountService _accountService;
		public PasswordChangePage ()
		{
		    InitializeComponent ();
		    _accountService = new AccountService();
		}

	    private async void BackClicked(object sender, EventArgs e)
	    {
	        await Navigation.PopAsync();
	    }

	    private async void SubmitClicked(object sender, EventArgs e)
	    {
	        try
	        {
	            await ActivityIndicatorPage.ToggleIndicator(true);
	            var passwordChangeDto = new PasswordChangeDto
	            {
	                CurrentPassword = OldPass.Text,
	                NewPassword = NewPass.Text,
	                NewPasswordRepeated = NewPassRepeated.Text
	            };
	            await _accountService.ChangePasswordAsync(passwordChangeDto);
	            await DisplayAlert("Success", "Success", "OK");
	            await Navigation.PopAsync();
	        }
	        catch (RestException exception)
	        {
	            await DisplayAlert("Error", exception.Message, "OK");
	        }
	        finally
	        {
	            await ActivityIndicatorPage.ToggleIndicator(false);
	        }
        }
	}
}