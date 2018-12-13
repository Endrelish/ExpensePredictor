using ExpensePrediction.Frontend.Service;
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
	public partial class IncomesPage : ContentPage
	{
        private readonly IncomeService _incomeService;
		public IncomesPage()
		{
			InitializeComponent ();
            _incomeService = new IncomeService();
            DateFrom.Date = DateTime.Now.AddMonths(-1);
            DateTo.Date = DateTime.Now;
        }

        private async void GetIncomesClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var incomes = await _incomeService.GetIncomesAsync(DateFrom.Date, DateTo.Date);
            IncomesList.ItemsSource = incomes.OrderByDescending(i => i.Date);
            await ActivityIndicatorPage.ToggleIndicator(false);
        }
    }
}