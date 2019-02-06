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
	public partial class PasswordChangePage : ContentPage
	{
		public PasswordChangePage ()
		{
			InitializeComponent ();
		}

	    private void BackClicked(object sender, EventArgs e)
	    {
            //TODO
	    }

	    private async void SubmitClicked(object sender, EventArgs e)
	    {
	        await DisplayAlert("Error", "New password must be at least 8 characters long.", "OK");
        }
	}
}