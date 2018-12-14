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
            ExpensesList.ItemsSource = expenses.OrderByDescending(ex => ex.Date);
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
    }
}