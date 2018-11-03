using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("/api/expense")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ExpenseController(UserManager<User> userManager,
            IMapper mapper, IExpenseService expenseService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _expenseService = expenseService;
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
            try
            {
                var expense = await _expenseService.AddExpense(expenseDto, User.Identity.Name);
                return CreatedAtRoute("GetExpense", new {expenseId = expense.Id}, _mapper.Map<ExpenseDto>(expense));
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom error codes and exceptions
            }
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
                var expense = await _expenseService.GetExpense(expenseId,
                    (await _userManager.FindByIdAsync(User.Identity.Name)).Id);
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
            var expenses = await _expenseService.GetExpenses(User.Identity.Name);

            return Ok(_mapper.Map<IEnumerable<ExpenseDto>>(expenses));
        }

        [HttpPost("edit")]
        [Authorize("EditExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> EditExpense([FromBody] ExpenseDto expenseDto)
        {
            try
            {
                var result = await _expenseService.EditExpense(expenseDto, User.Identity.Name);
                return Ok(result);
            }
            catch (Exception e) //TODO Custom exceptions
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet("linked/{expenseId}")]
        [Authorize("GetExpenses")]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetLinkedExpenses([FromRoute] string expenseId)
        {
            try
            {
                var expenses = await _expenseService.GetLinkedExpenses(expenseId, User.Identity.Name);
                return Ok(expenses);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom exceptions
            }
        }
    }
}