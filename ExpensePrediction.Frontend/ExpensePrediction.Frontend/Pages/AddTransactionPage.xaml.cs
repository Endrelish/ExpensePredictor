using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
using ExpensePrediction.Frontend.Service;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddTransactionPage : ContentPage, IInitializedPage
    {
        private readonly CategoryType _type;
        private readonly CategoryService _categoryService;
        private readonly ExpenseService _expenseService;
        private readonly IncomeService _incomeService;
        private readonly Action<TransactionDto> _addTransaction;
        public AddTransactionPage (CategoryType type, Action<TransactionDto> addTransaction)
		{
			InitializeComponent();
            _type = type;
            _expenseService = new ExpenseService();
            _incomeService = new IncomeService();
            _categoryService = new CategoryService();
            _addTransaction = addTransaction;

            switch(type)
            {
                case CategoryType.ExpenseCategory:
                    Title = "Add expense";
                    break;
                case CategoryType.IncomeCategory:
                    Title = "Add income";
                    break;
            }
		}

        public async Task Initialize()
        {
            Category.ItemsSource = await _categoryService.GetCategories(_type);
            Category.SelectedItem = Category.ItemsSource[0];
        }

        private async void AddTransactionClicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var dto = new TransactionDto
            {
                Description = Description.Text,
                Value = double.Parse(Value.Text),
                Date = Date.Date,
                CategoryId = ((CategoryDto)Category.SelectedItem).Id
            };

            switch(_type)
            {
                case CategoryType.IncomeCategory:
                    await _incomeService.AddIncomeAsync(dto);
                    break;
                case CategoryType.ExpenseCategory:
                    await _expenseService.AddExpenseAsync(dto);
                    break;
            }
            await ActivityIndicatorPage.ToggleIndicator(false);
            await Close();
        }

        private async void CancelClicked(object sender, EventArgs e)
        {
            await Close();
        }

        private async Task Close()
        {
            await Navigation.PopAsync();
        }
    }
}