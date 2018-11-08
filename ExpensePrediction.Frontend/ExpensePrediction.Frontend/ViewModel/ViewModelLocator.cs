using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensePrediction.Frontend.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel _mainViewModel;

        public MainViewModel MainViewModel { get => _mainViewModel; }
    }
}
