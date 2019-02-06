using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
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

        public EditTransactionPage (CategoryType type, TransactionDto dto)
		{
			InitializeComponent ();
		    _type = type;
		    _expenseService = new ExpenseService();
		    _incomeService = new IncomeService();
		    _categoryService = new CategoryService();
		    _dto = dto;

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

	    private void SubmitClicked(object sender, EventArgs e)
	    {
            //TODO
	    }

	    private void BackClicked(object sender, EventArgs e)
	    {
	        //TODO
	    }

        public async Task Initialize()
        {
            var items = await _categoryService.GetCategories(_type);
            Category.ItemsSource = items;
            Category.SelectedItem = items.FirstOrDefault(i => i.Id == _dto.CategoryId);
        }
    }
}