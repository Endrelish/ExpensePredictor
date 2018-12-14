using ExpensePrediction.DataTransferObjects;
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
        private void AddIncome(TransactionDto transaction)
        {
            if (transaction is IncomeDto income)
            {
                var incomes = IncomesList.ItemsSource.Cast<IncomeDto>().ToList();
                incomes.Add(income);
                IncomesList.ItemsSource = incomes;
            }
        }
        private async void AddIncomeClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var page = new AddTransactionPage(DataTransferObjects.Category.CategoryType.IncomeCategory, AddIncome);
            await page.Initialize();
            await ActivityIndicatorPage.ToggleIndicator(false);
            await Navigation.PushAsync(page);
        }
    }
}