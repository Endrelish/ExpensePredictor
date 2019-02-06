using ExpensePrediction.DataTransferObjects.User;
using ExpensePrediction.Frontend.Service;
using Rg.Plugins.Popup.Services;
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
    public partial class UserDetailPage : ContentPage, IInitializedPage
    {
        private readonly UserService _userService;
        public UserDataDto UserData { get; }
        public UserDetailPage()
        {
            InitializeComponent();
            _userService = new UserService();
            UserData = new UserDataDto();
        }

        private void SetUserData(UserDataDto result)
        {
            FirstName.Text = result.FirstName;
            LastName.Text = result.LastName;
            PhoneNumber.Text = result.PhoneNumber;
            Username.Text = result.Username;
        }
        
        public async Task Initialize()
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var data = await _userService.GetUserDetailsAsync();
            SetUserData(data);
            await ActivityIndicatorPage.ToggleIndicator(false);
        }

        private async void EditClicked(object sender, EventArgs e)
        {
            var page = new UserEditPage(FirstName.Text, LastName.Text, PhoneNumber.Text);
            await Navigation.PushAsync(page);
            await Initialize();
        }

        private async void PassClicked(object sender, EventArgs e)
        {
            var page = new PasswordChangePage();
            await Navigation.PushAsync(page);
        }
    }
}