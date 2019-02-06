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
	public partial class UserEditPage : ContentPage
	{
	    public UserEditPage(string firstName, string lastName, string phone)
	    {
            InitializeComponent();
	        FirstName.Text = firstName;
	        LastName.Text = lastName;
	        Phone.Text = phone;
	    }

	    private void BackClicked(object sender, EventArgs e)
	    {
            //TODO
	    }

	    private void SubmitClicked(object sender, EventArgs e)
	    {
            //TODO
	    }
    }
}