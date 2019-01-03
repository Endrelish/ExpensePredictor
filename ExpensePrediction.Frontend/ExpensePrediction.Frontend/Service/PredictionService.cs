using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.Frontend.Service
{
    public class PredictionService
    {
        public async Task<PredictionResultDto> MakePrediction(ExpenseDto expense)
        {
            return await RestService.PostAsync<PredictionResultDto>(Constants.PredictionUri, expense);
        }
    }
}
