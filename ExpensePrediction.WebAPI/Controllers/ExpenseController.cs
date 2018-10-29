using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using ExpensePrediction.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("/api/expense")]
    public class ExpenseController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IApplicationRepository<ExpenseCategory> _expenseCategoryRepository;
        private readonly IApplicationRepository<Expense> _expenseRepository;

        public ExpenseController(UserManager<User> userManager,
            IMapper mapper, IApplicationRepository<ExpenseCategory> expenseCategoryRepository, IApplicationRepository<Expense> expenseRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseRepository = expenseRepository;
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

            return CreatedAtRoute("GetExpense", new {expenseId = expense.Id}, _mapper.Map<ExpenseDto>(expense));
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
            var expense = await _expenseRepository.FindByIdAsync(expenseId);
            if (expense == null) return NotFound();
            var dto = _mapper.Map<ExpenseDto>(expense);
            return Ok(dto);
        }

        [HttpGet]
        [Authorize("GetExpenses")]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetExpenses()
        {
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