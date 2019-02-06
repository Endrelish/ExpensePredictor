using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.Frontend.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects.Category;
using ExpensePrediction.Exceptions;
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
            IncomesList.ItemsSource = incomes.OrderByDescending(i => i.Date)
                .Select(i => new {i.Description, Value = i.Value.ToString("F2") + " zł", i.Date, i.Id, i.CategoryId});
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

	    private async void IncomeClicked(object sender, ItemTappedEventArgs e)
	    {
	        var action = await DisplayActionSheet("Options", "Back", null, new[] {"Edit", "Delete"});
	        dynamic item = e.Item;
	        var dto = new TransactionDto()
	        {
	            CategoryId = item.CategoryId,
	            Date = item.Date,
	            Description = item.Description,
	            Id = item.Id,
	            Value = double.Parse(item.Value.Substring(0, item.Value.Length - 3))
	        };
	        try
	        {
	            switch (action)
	            {
	                case "Edit":
	                    await ActivityIndicatorPage.ToggleIndicator(true);
	                    var page = new EditTransactionPage(CategoryType.IncomeCategory, dto, () => GetIncomesClicked(null, null));
	                    await page.Initialize();
	                    await Navigation.PushAsync(page);
	                    break;
	                case "Delete":
                        var choice = await DisplayAlert("Delete", "Are you sure?", "Yes", "No");
	                    if (choice)
	                    {
	                        await ActivityIndicatorPage.ToggleIndicator(true);
	                        await _incomeService.DeleteIncomeAsync(dto.Id);
	                    }
	                    break;
	            }
	                        GetIncomesClicked(sender, e);
	        }
	        catch (RestException exception)
	        {
	            await DisplayAlert("Error", exception.Message, "OK");
	        }
	        finally
	        {
	            await ActivityIndicatorPage.ToggleIndicator(false);
	        }
	        
	    }
	}
}