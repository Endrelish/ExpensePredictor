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

namespace ExpensePrediction.BusinessLogicLayer.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IApplicationRepository<IncomeCategory> _categoryRepository;
        private readonly IApplicationRepository<Income> _incomeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public IncomeService(IApplicationRepository<Income> incomeRepository, IMapper mapper,
            UserManager<User> userManager, IApplicationRepository<IncomeCategory> categoryRepository)
        {
            _incomeRepository = incomeRepository;
            _mapper = mapper;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }

        public async Task<IncomeDto> AddIncomeAsync(IncomeDto incomeDto, string userId)
        {
            var income = _mapper.Map<Income>(incomeDto);
            income.Id = null;
            income.UserId = userId;
            await _incomeRepository.CreateAsync(income);
            var result = await _incomeRepository.SaveAsync();
            //if(result == 0) throw sth;
            return _mapper.Map<IncomeDto>(income);
        }

        public async Task<IncomeDto> EditIncomeAsync(IncomeDto incomeDto, string userId)
        {
            var income = await _incomeRepository.FindByIdAsync(incomeDto.Id);
            if (!income.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new IncomeException("Cannot find income", 400);
            }
            
            income.Value = incomeDto.Value;
            income.Date = incomeDto.Date;
            income.CategoryId = incomeDto.CategoryId;

            var result = await _incomeRepository.SaveAsync();
            if (result < 1)
            {
                throw new IncomeException("Cannot edit income", 500);
            }

            return _mapper.Map<IncomeDto>(income);
        }

        public async Task<IncomeDto> GetIncomeAsync(string incomeId, string userId)
        {
            var income = await _incomeRepository.FindByIdAsync(incomeId);

            if (income != null && income.UserId == userId)
            {
                return _mapper.Map<IncomeDto>(income);
            }

            throw new IncomeException("Income not found", 400);
        }

        public async Task<IEnumerable<IncomeDto>> GetIncomesAsync(string userId, DateTime from, DateTime to)
        {
            Expression<Func<Income, bool>> condition = i => i.UserId == userId &&
                                                            i.Date >= from &&
                                                            i.Date <= to;

            var incomes = await _incomeRepository.FindByConditionAync(condition);
            return _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }
    }
}