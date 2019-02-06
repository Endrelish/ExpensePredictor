using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
using ExpensePrediction.Exceptions;
using ExpensePrediction.Frontend.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTransactionPage : ContentPage, IInitializedPage
    {
        private readonly CategoryType _type;
        private readonly ExpenseService _expenseService;
        private readonly IncomeService _incomeService;
        private readonly CategoryService _categoryService;
        private TransactionDto _dto;
        private readonly Action _refresh;

        public EditTransactionPage (CategoryType type, TransactionDto dto, Action refresh)
		{
			InitializeComponent ();
		    _type = type;
		    _expenseService = new ExpenseService();
		    _incomeService = new IncomeService();
		    _categoryService = new CategoryService();
		    _dto = dto;
		    _refresh = refresh;

		    Date.Date = dto.Date;
		    Description.Text = dto.Description;
		    Value.Text = dto.Value.ToString("F2");
            

		    switch (type)
		    {
		        case CategoryType.ExpenseCategory:
		            Title = "Edit expense";
		            break;
		        case CategoryType.IncomeCategory:
		            Title = "Edit income";
		            break;
		    }
        }

	    private async void SubmitClicked(object sender, EventArgs e)
	    {
	        var dto = new TransactionDto
	        {
	            CategoryId = ((CategoryDto) Category.SelectedItem).Id,
	            Date = Date.Date.Date,
	            Description = Description.Text,
	            Id = _dto.Id,
	            Value = double.Parse(Value.Text)
	        };
            
	            await ActivityIndicatorPage.ToggleIndicator(true);
	            try
	            {
	                if (_type == CategoryType.IncomeCategory)
	                    await _incomeService.EditIncome(dto);
	                else
	                    await _expenseService.EditExpense(dto);

	                _refresh();
	                BackClicked(sender, e);
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

	    private async void BackClicked(object sender, EventArgs e)
	    {
	        await Navigation.PopAsync();
	    }

        public async Task Initialize()
        {
            var items = await _categoryService.GetCategories(_type);
            Category.ItemsSource = items;
            Category.SelectedItem = items.FirstOrDefault(i => i.Id == _dto.CategoryId);
        }
    }
}