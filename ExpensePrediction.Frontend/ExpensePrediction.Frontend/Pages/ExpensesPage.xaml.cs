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
	public partial class ExpensesPage : ContentPage
	{
        private readonly ExpenseService _expenseService;
		public ExpensesPage ()
		{
			InitializeComponent ();
            _expenseService = new ExpenseService();
            DateFrom.Date = DateTime.Now.AddMonths(-1);
            DateTo.Date = DateTime.Now;
		}

        private async void GetExpensesClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var expenses = await _expenseService.GetExpenses(DateFrom.Date, DateTo.Date);
            ExpensesList.ItemsSource = expenses.OrderByDescending(ex => ex.Date)
                .Select(i => new { i.Description, Value = i.Value.ToString("F2") + " zł", i.Date, i.Id, i.CategoryId });
            await ActivityIndicatorPage.ToggleIndicator(false);
        }

        private void AddExpense(TransactionDto transaction)
        {
            if (transaction is ExpenseDto expense)
            {
                var incomes = ExpensesList.ItemsSource.Cast<ExpenseDto>().ToList();
                incomes.Add(expense);
                ExpensesList.ItemsSource = incomes;
            }
        }

        private async void AddExpenseClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var page = new AddTransactionPage(DataTransferObjects.Category.CategoryType.ExpenseCategory, AddExpense);
            await page.Initialize();
            await ActivityIndicatorPage.ToggleIndicator(false);
            await Navigation.PushAsync(page);
        }

        private async void ExpenseClicked(object sender, ItemTappedEventArgs e)
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
	                    var page = new EditTransactionPage(CategoryType.ExpenseCategory, dto, () => GetExpensesClicked(null, null));
	                    await page.Initialize();
	                    await ActivityIndicatorPage.ToggleIndicator(false);
	                    await Navigation.PushAsync(page);
	                    break;
	                case "Delete":
                        var choice = await DisplayAlert("Delete", "Are you sure?", "Yes", "No");
	                    if (choice)
	                    {
	                        await ActivityIndicatorPage.ToggleIndicator(true);
	                        await _expenseService.DeleteExpenseAsync(dto.Id);
                            GetExpensesClicked(sender, e);
	                    }
	                    break;
	            }
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