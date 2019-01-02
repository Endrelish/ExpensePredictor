using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.Exceptions;
using Microsoft.AspNetCore.Identity;
using ApplicationException = System.ApplicationException;

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
            try
            {
                var expense = _mapper.Map<Expense>(expenseDto);
                expense.Id = null;
                expense.Date = expense.Date.Date;
                expense.UserId = userId;
                await _expenseRepository.CreateAsync(expense);
                await _expenseRepository.SaveAsync();
                return _mapper.Map<ExpenseDto>(expense);
            }
            catch (RepositoryException e)
            {
                throw new ExpenseException("Cannot add expense", e, 500);
            }
            
        }

        public async Task<ExpenseDto> EditExpenseAsync(ExpenseDto expenseDto, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseDto.Id);
            if (!expense.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new ExpenseException("Cannot find expense", 400);
            }
            
            expense.Value = expenseDto.Value;
            expense.Date = expenseDto.Date;
            expense.CategoryId = expenseDto.CategoryId;

            var result = await _expenseRepository.SaveAsync();
            if (result < 1)
            {
                throw new ExpenseException("Cannot edit expense", 500);
            }

            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<ExpenseDto> GetExpenseAsync(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (expense != null && expense.UserId == userId)
            {
                return _mapper.Map<ExpenseDto>(expense);
            }

            throw new ExpenseException("Cannot find expense", 400);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync(string userId, DateTime from, DateTime to)
        {
            Expression<Func<Expense, bool>> condition = e => e.UserId == userId &&
                                                             e.Date >= from &&
                                                             e.Date <= to;

            var expenses = await _expenseRepository.FindByConditionAync(condition);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDto>> GetLinkedExpensesAsync(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (!string.Equals(expense.UserId, userId))
            {
                throw new ExpenseException("Cannot find expense", 400);
            }

            var linkedExpenses = await _expenseRepository.FindByConditionAync(e => e.LinkedExpenseId == expenseId);
            return _mapper.Map<IEnumerable<ExpenseDto>>(linkedExpenses);
        }
    }
}