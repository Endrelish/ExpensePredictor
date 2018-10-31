using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IApplicationRepository<ExpenseCategory> _categoryRepository;

        public ExpenseService(IApplicationRepository<Expense> expenseRepository, IMapper mapper, UserManager<User> userManager, IApplicationRepository<ExpenseCategory> categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }
        public async Task<ExpenseDto> AddExpense(ExpenseDto expenseDto, string userId)
        {
            var expense = _mapper.Map<Expense>(expenseDto);
            expense.Id = null;
            //TODO check if specified keys exist in the db
            expense.UserId = userId;
            await _expenseRepository.CreateAsync(expense);
            var result = await _expenseRepository.SaveAsync(); //TODO Check if saved
            //if(result == 0) throw sth;
            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<ExpenseDto> EditExpense(ExpenseDto expenseDto, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpenseDto> GetExpense(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (expense == null) throw new Exception(); //TODO throw not found sth
            if (expense.UserId == userId) return _mapper.Map<ExpenseDto>(expense);
            throw new Exception(); //TODO custom exceptions
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpenses(string userId)
        {
            var expenses = await _expenseRepository.FindByConditionAync(e => e.User.Id == userId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }
    }
}
