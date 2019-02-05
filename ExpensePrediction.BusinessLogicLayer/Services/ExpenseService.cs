using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataAccessLayer.Interfaces;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IApplicationRepository<Income> _incomeRepository;
        private readonly IApplicationRepository<Expense> _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IApplicationRepository<Expense> expenseRepository, IMapper mapper, IApplicationRepository<Income> incomeRepository)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _incomeRepository = incomeRepository;
        }

        public async Task<ExpenseDto> AddExpenseAsync(ExpenseDto expenseDto, string userId)
        {
            try
            {
                var expense = _mapper.Map<Expense>(expenseDto);
                expense.Id = null;
                expense.Date = expense.Date.Date;
                expense.UserId = userId;
                expense.Main = await IsExpenseMain(expenseDto, userId);
                await _expenseRepository.CreateAsync(expense);
                await _expenseRepository.SaveAsync();
                return _mapper.Map<ExpenseDto>(expense);
            }
            catch (RepositoryException e)
            {
                throw new ExpenseException("Cannot add expense", e, 400);
            }
        }

        private async Task<bool> IsExpenseMain(ExpenseDto expenseDto, string userId)
        {
            var incomes = (await _incomeRepository.FindByConditionAsync(i => i.UserId == userId &&
                                                                            i.Date.Month == expenseDto.Date
                                                                                .AddMonths(-1).Month &&
                                                                            i.Date.Year == expenseDto.Date.AddMonths(-1)
                                                                                .Year)).Sum(i => i.Value);
            return expenseDto.Value >= 0.9d * incomes;
        }

        public async Task<ExpenseDto> EditExpenseAsync(ExpenseDto expenseDto, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseDto.Id);
            if (!expense.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new ExpenseException("Cannot find expense", 404);
            }

            expense.Value = expenseDto.Value;
            expense.Date = expenseDto.Date;
            expense.CategoryId = expenseDto.CategoryId;

            var result = await _expenseRepository.SaveAsync();
            if (result < 1)
            {
                throw new ExpenseException("Cannot edit expense", 400);
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

            throw new ExpenseException("Cannot find expense", 404);
        }

        public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync(string userId, DateTime from, DateTime to)
        {
            Expression<Func<Expense, bool>> condition = e => e.UserId == userId &&
                                                             e.Date >= from &&
                                                             e.Date <= to;

            var expenses = await _expenseRepository.FindByConditionAsync(condition);
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task DeleteExpenseAsync(string expenseId, string userId)
        {
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (expense != null && expense.UserId == userId)
            {
                _expenseRepository.Delete(expense);
                await _expenseRepository.SaveAsync();
                return;
            }

            throw new ExpenseException("Cannot find expense", 404);
        }
    }
}