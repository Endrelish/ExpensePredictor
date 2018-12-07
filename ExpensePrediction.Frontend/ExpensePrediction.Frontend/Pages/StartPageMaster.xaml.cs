using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExpensePrediction.Frontend.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPageMaster : ContentPage
    {
        public ListView ListView;

        public StartPageMaster()
        {
            InitializeComponent();

            BindingContext = new StartPageMasterViewModel();
            ListView = MenuItemsListView;
        }
        
        public void LogOut(object sender, EventArgs eventArgs)
        {
            var authService = new AuthService();

            void Navigate()
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }

            authService.Logout(Navigate);
        }

        private class StartPageMasterViewModel : INotifyPropertyChanged
        {
            public StartPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<StartPageMenuItem>(new[]
                {
                    new StartPageMenuItem (0, "User details", typeof(UserDetailPage)),
                    new StartPageMenuItem (0, "Page 2", typeof(StartPageDetail)),
                    new StartPageMenuItem (0, "Page 3", typeof(StartPageDetail)),
                });
            }

            public ObservableCollection<StartPageMenuItem> MenuItems { get; }

            #region INotifyPropertyChanged Implementation

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                {
                    return;
                }

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }
    }
}