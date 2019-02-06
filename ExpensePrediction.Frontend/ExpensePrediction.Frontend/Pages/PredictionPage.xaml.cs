using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.DataTransferObjects.Category;
using ExpensePrediction.Frontend.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.Exceptions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensePrediction.Frontend.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionPage : ContentPage, IInitializedPage
    {
        private readonly CategoryService _categoryService;
        private readonly PredictionService _predictionService;
        public PredictionPage()
        {
            InitializeComponent();
            _categoryService = new CategoryService();
            _predictionService = new PredictionService();
        }

        public async Task Initialize()
        {
            Category.ItemsSource = await _categoryService.GetCategories(DataTransferObjects.Category.CategoryType.ExpenseCategory);
            Category.SelectedItem = Category.ItemsSource[0];
        }

        private void ShowResult(PredictionResultDto dto)
        {
            FirstMonthLabel.Text = Date.Date.AddMonths(1).ToString("MMMM");
            SecondMonthLabel.Text = Date.Date.AddMonths(2).ToString("MMMM");
            ThirdMonthLabel.Text = Date.Date.AddMonths(3).ToString("MMMM");

            FirstMonthValue.Text = dto.FirstMonthValue.ToString("F2") + " zł";
            SecondMonthValue.Text = dto.SecondMonthValue.ToString("F2") + " zł";
            ThirdMonthValue.Text = dto.ThirdMonthValue.ToString("F2") + " zł";

            PredictionGrid.IsVisible = true;
        }

        private async void PredictionButton_Clicked(object sender, EventArgs e)
        {
            await ActivityIndicatorPage.ToggleIndicator(true);
            var dto = new ExpenseDto
            {
                Value = double.Parse(Value.Text),
                Date = Date.Date,
                CategoryId = ((CategoryDto)Category.SelectedItem).Id
            };

            try
            {
                var result = await _predictionService.MakePrediction(dto);
                ShowResult(result);
            }
            catch (RestException exc)
            {
                await DisplayAlert("Error", exc.Message, "OK");
            }
            finally
            {
                await ActivityIndicatorPage.ToggleIndicator(false);
            }
        }
    }
}