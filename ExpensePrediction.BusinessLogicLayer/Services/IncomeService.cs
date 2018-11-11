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
    public class IncomeService : IIncomeService
    {
        private readonly IApplicationRepository<Income> _incomeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IApplicationRepository<IncomeCategory> _categoryRepository;

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
            //TODO check if specified keys exist in the db
            income.UserId = userId;
            await _incomeRepository.CreateAsync(income);
            var result = await _incomeRepository.SaveAsync(); //TODO Check if saved
            //if(result == 0) throw sth;
            return _mapper.Map<IncomeDto>(income);
        }

        public async Task<IncomeDto> EditIncomeAsync(IncomeDto incomeDto, string userId)
        {
            var income = await _incomeRepository.FindByIdAsync(incomeDto.Id);
            if (!income.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("NOT_YOUR_EXPENSE_YOU_DAMN_HACKER"); //TODO throw custom exceptions
            }

            //TODO check if specified keys exist
            income.Value = incomeDto.Value;
            income.Date = incomeDto.Date;
            income.CategoryId = incomeDto.CategoryId;

            var result = await _incomeRepository.SaveAsync();
            if (result < 1)
            {
                throw new Exception("SOMETHING_WENT_WRONG"); //TODO throw custom exceptions
            }

            return _mapper.Map<IncomeDto>(income);
        }

        public async Task<IncomeDto> GetIncomeAsync(string incomeId, string userId)
        {
            var income = await _incomeRepository.FindByIdAsync(incomeId);
            if (income == null)
            {
                throw new Exception(); //TODO throw not found sth
            }

            if (income.UserId == userId)
            {
                return _mapper.Map<IncomeDto>(income);
            }

            throw new Exception(); //TODO custom exceptions
        }

        public async Task<IEnumerable<IncomeDto>> GetIncomesAsync(string userId)
        {
            var incomes = await _incomeRepository.FindByConditionAync(i => i.User.Id == userId);
            return _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        }
    }
}