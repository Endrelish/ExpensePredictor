using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IApplicationRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IApplicationRepository<Expense> expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }
        public async Task<ExpenseDto> AddExpense(ExpenseDto expense)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpenseDto> EditExpense(ExpenseDto expense)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpenseDto> GetExpense(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (expense == null) throw new Exception(); //TODO throw not found sth
            if (expense.User.Id == userId) return _mapper.Map<ExpenseDto>(expense);
            throw new Exception(); //TODO custom exceptions
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpenses(string userId)
        {
            var expenses = await _expenseRepository.FindByConditionAync(e => e.User.Id == userId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }
}
