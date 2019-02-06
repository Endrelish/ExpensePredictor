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
	public partial class UserEditPage : ContentPage
	{
	    private readonly UserService _userService;
	    public UserEditPage(string firstName, string lastName, string phone)
	    {
            InitializeComponent();
	        FirstName.Text = firstName;
	        LastName.Text = lastName;
	        Phone.Text = phone;
	        _userService = new UserService();
	    }

	    private async void BackClicked(object sender, EventArgs e)
	    {
	        await Navigation.PopAsync();
	    }

	    private async void SubmitClicked(object sender, EventArgs e)
	    {
	        await ActivityIndicatorPage.ToggleIndicator(true);
	        var dto = new UserEditDto
	        {
	            FirstName = FirstName.Text,
	            LastName = LastName.Text,
	            PhoneNumber = Phone.Text
	        };
	        try
	        {
	            await _userService.EditUser(dto);
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