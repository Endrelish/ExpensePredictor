using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Interfaces.Services
{
    public interface IIncomeService
    {
        Task<IncomeDto> GetIncomeAsync(string incomeId, string userId);

        Task<IEnumerable<IncomeDto>> GetIncomesAsync(string userId, DateTime from, DateTime to);

        Task<IncomeDto> AddIncomeAsync(IncomeDto income, string userId);

        Task<IncomeDto> EditIncomeAsync(IncomeDto incomeDto, string userId);

        Task DeleteIncomeAsync(string incomeId, string uesrId);
    }
}