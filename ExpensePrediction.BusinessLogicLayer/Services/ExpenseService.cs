using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IApplicationRepository<ExpenseCategory> _categoryRepository;
        private readonly IApplicationRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ExpenseService(IApplicationRepository<Expense> expenseRepository, IMapper mapper,
            UserManager<User> userManager, IApplicationRepository<ExpenseCategory> categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }

        public async Task<ExpenseDto> AddExpenseAsync(ExpenseDto expenseDto, string userId)
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

        public async Task<ExpenseDto> EditExpenseAsync(ExpenseDto expenseDto, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseDto.Id);
            if (!expense.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("NOT_YOUR_EXPENSE_YOU_DAMN_HACKER"); //TODO throw custom exceptions
            }

            //TODO check if specified keys exist
            expense.Value = expenseDto.Value;
            expense.Date = expenseDto.Date;
            expense.CategoryId = expenseDto.CategoryId;
            expense.LinkedExpenseId = expenseDto.LinkedExpenseId;

            var result = await _expenseRepository.SaveAsync();
            if (result < 1)
            {
                throw new Exception("SOMETHING_WENT_WRONG"); //TODO throw custom exceptions
            }

            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<ExpenseDto> GetExpenseAsync(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (expense == null)
            {
                throw new Exception(); //TODO throw not found sth
            }

            if (expense.UserId == userId)
            {
                return _mapper.Map<ExpenseDto>(expense);
            }

            throw new Exception(); //TODO custom exceptions
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync(string userId)
        {
            var expenses = await _expenseRepository.FindByConditionAync(e => e.User.Id == userId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDto>> GetLinkedExpensesAsync(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (!string.Equals(expense.UserId, userId))
            {
                throw new Exception("NOT_YOUR_EXPENSE_YOU_DAMN_HACKER"); //TODO throw custom exceptions
            }

            var linkedExpenses = await _expenseRepository.FindByConditionAync(e => e.LinkedExpenseId == expenseId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(linkedExpenses);
        }
    }
}