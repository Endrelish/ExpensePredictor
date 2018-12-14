using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.Frontend.Service
{
    public class IncomeService
    {
        public async Task<List<IncomeDto>> GetIncomesAsync(DateTime from, DateTime to)
        {
            return await RestService.GetAsync<List<IncomeDto>>(Constants.GetIncomesUri(from, to));
        }

        public async Task<IncomeDto> AddIncomeAsync(TransactionDto dto)
        {
            return await RestService.PostAsync<IncomeDto>(Constants.AddIncomeUri, dto);
        }
    }
}
