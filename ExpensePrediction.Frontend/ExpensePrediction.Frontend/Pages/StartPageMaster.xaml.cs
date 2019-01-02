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
            void Navigate()
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }

            AuthService.Logout(Navigate);
        }

        private class StartPageMasterViewModel : INotifyPropertyChanged
        {
            public StartPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<StartPageMenuItem>(new[]
                {
                    new StartPageMenuItem (0, "User details", typeof(UserDetailPage)),
                    new StartPageMenuItem (1, "Expenses", typeof(ExpensesPage)),
                    new StartPageMenuItem (2, "Incomes", typeof(IncomesPage)),
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