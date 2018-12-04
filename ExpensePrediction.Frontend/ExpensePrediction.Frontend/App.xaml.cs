using System.Threading.Tasks;
using ExpensePrediction.Frontend.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ExpensePrediction.Frontend
{
    public partial class App : Application
    {
        public App()
        {
            var token = Task.Run(async () => await SecureStorage.GetAsync(Constants.Token));
            IsUserLoggedIn = token.Result != null;
            InitializeComponent();

            if (IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new StartPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        public static bool IsUserLoggedIn { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}