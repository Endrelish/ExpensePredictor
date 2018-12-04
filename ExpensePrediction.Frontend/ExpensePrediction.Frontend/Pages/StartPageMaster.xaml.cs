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
                    new StartPageMenuItem {Id = 0, Title = "Page 1"},
                    new StartPageMenuItem {Id = 1, Title = "Page 2"},
                    new StartPageMenuItem {Id = 2, Title = "Page 3"},
                    new StartPageMenuItem {Id = 3, Title = "Page 4"},
                    new StartPageMenuItem {Id = 4, Title = "Page 5"}
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