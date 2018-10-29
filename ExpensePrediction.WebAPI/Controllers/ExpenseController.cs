using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("/api/expense")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly User _user;
        private readonly UserManager<User> _userManager;
        private readonly IApplicationRepository<ExpenseCategory> _expenseCategoryRepository;
        private readonly IExpenseService _expenseService;

        public ExpenseController(UserManager<User> userManager,
            IMapper mapper, IApplicationRepository<ExpenseCategory> expenseCategoryRepository, IExpenseService expenseService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseService = expenseService;
            _user = userManager.FindByNameAsync(User.Identity.Name);
        }

        /// <summary>
        ///     Adds an expense.
        /// </summary>
        /// <consumes>application/json</consumes>
        /// <param name="expenseDto">The expense data.</param>
        /// <returns>Link to the created expense</returns>
        [HttpPost("add-expense")]
        [Authorize("AddExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
        {
            expenseDto.Id = string.Empty;
            var expense = _mapper.Map<Expense>(expenseDto);
            expense.User = await _userManager.FindByNameAsync(User.Identity.Name);
            expense.Category = await _expenseCategoryRepository.FindByIdAsync(expenseDto.CategoryId);
            if (!string.IsNullOrEmpty(expenseDto.LinkedExpenseId))
            {
                expense.LinkedExpense = await _expenseRepository.FindByIdAsync(expenseDto.LinkedExpenseId);
            }

            await _expenseRepository.CreateAsync(expense);
            await _expenseRepository.SaveAsync(); //TODO catch sth I guess

            return CreatedAtRoute("GetExpense", new { expenseId = expense.Id }, _mapper.Map<ExpenseDto>(expense));
        }

        /// <summary>
        ///     Gets the expense data.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <returns></returns>
        [HttpGet("{expenseId}", Name = "GetExpense")]
        [Authorize("GetExpense")]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetExpense([FromRoute] string expenseId)
        {
            try
            {
                var expense = _expenseService.GetExpense(expenseId, (await _userManager.FindByNameAsync(User.Identity.Name)).Id);
                return Ok(expense);
            }
            catch (Exception) //TODO Custom exceptions
            {
                return StatusCode(400, "ERROR"); //TODO Custom error codes
            }
        }

        [HttpGet]
        [Authorize("GetExpenses")]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetExpenses()
        {
            var expenses = _expenseService.GetExpenses(_user.Id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var expenses = _expenseRepository.FindByConditionAync(e => e.User.Id == user.Id);

            return Ok(_mapper.Map<IEnumerable<ExpenseDto>>(expenses));
        }

        [HttpPut("edit")]
        [Authorize("EditExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> EditExpense([FromBody] ExpenseDto expenseDto)
        {
            throw new NotImplementedException();
        }
    }
}