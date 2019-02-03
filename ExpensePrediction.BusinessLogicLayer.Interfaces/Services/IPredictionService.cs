using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExpensePrediction.DataTransferObjects;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IPredictionService
    {
        Task<PredictionResultDto> Prediction(ExpenseDto expenseDto, string userId);
        Task SetModels(string categoryId);
    }
}
