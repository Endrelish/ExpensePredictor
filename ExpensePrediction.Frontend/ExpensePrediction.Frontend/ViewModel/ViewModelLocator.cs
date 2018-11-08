namespace ExpensePrediction.Frontend.ViewModel
{
    public class ViewModelLocator
    {
        private static readonly MainViewModel MainViewModelInstance = new MainViewModel();

        public MainViewModel MainViewModel => MainViewModelInstance;
    }
}