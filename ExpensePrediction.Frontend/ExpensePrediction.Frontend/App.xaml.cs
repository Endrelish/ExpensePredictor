using ExpensePrediction.Frontend.Pages;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ExpensePrediction.Frontend
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }

        public App()
        {
            var token = Task.Run(async () => await SecureStorage.GetAsync(Constants.Token));
            IsUserLoggedIn = token.Result != null;
            InitializeComponent();

            if (IsUserLoggedIn)
            {
                MainPage = new StartPage();
            }
            else
            {
                MainPage = new MainPage();
            }
        }

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