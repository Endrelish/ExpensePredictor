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
        /// Adds an expense.
        /// </summary>
        /// <param name="expenseDto">The expense data.</param>
        /// <returns>
        /// Link to the created expense.
        /// </returns>
        /// <consumes>application/json</consumes>
        [HttpPost("add")]
        [Authorize("AddExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(ExpenseDto), 201)]
        [ProducesResponseType(typeof(string), 400)] //TODO custom exceptions
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
        {
            try
            {
                var expense = await _expenseService.AddExpenseAsync(expenseDto, User.Identity.Name);
                return CreatedAtRoute("GetExpense", new {expenseId = expense.Id}, expense);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom error codes and exceptions
            }
        }

        /// <summary>
        /// Gets data of an expense specified by its id.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <returns>The expense.</returns>
        [HttpGet("{expenseId}", Name = "GetExpense")]
        [Authorize("GetExpense")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(ExpenseDto), 200)]
        [ProducesResponseType(typeof(string), 400)] //TODO custom exceptions
        public async Task<IActionResult> GetExpense([FromRoute] string expenseId)
        {
            try
            {
                var expense = await _expenseService.GetExpenseAsync(expenseId,
                    (await _userManager.FindByIdAsync(User.Identity.Name)).Id);
                return Ok(expense);
            }
            catch (Exception) //TODO Custom exceptions
            {
                return StatusCode(400, "ERROR"); //TODO Custom error codes
            }
        }

        /// <summary>
        /// Gets user expenses from given period of time.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns>Expenses.</returns>
        [HttpGet]
        [Authorize("GetExpenses")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), 200)]
        [ProducesResponseType(typeof(string), 400)] //TODO custom exceptions
        public async Task<IActionResult> GetExpenses([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var expenses = await _expenseService.GetExpensesAsync(User.Identity.Name, from, to);
                return Ok(expenses);
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR"); //TODO custom exceptions
            }
        }

        /// <summary>
        /// Edits the expense.
        /// </summary>
        /// <param name="expenseDto">The expense dto.</param>
        /// <returns>Edited expense.</returns>
        [HttpPost("edit")]
        [Authorize("EditExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(ExpenseDto), 200)]
        [ProducesResponseType(typeof(string), 400)] //TODO custom exceptions
        public async Task<IActionResult> EditExpense([FromBody] ExpenseDto expenseDto)
        {
            try
            {
                var result = await _expenseService.EditExpenseAsync(expenseDto, User.Identity.Name);
                return Ok(result);
            }
            catch (Exception e) //TODO Custom exceptions
            {
                return StatusCode(400, e.Message);
            }
        }

        /// <summary>
        /// Gets the linked expenses.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <returns>Linked expenses.</returns>
        [HttpGet("linked/{expenseId}")]
        [Authorize("GetExpenses")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), 200)]
        [ProducesResponseType(typeof(string), 400)] //TODO custom exceptions
        public async Task<IActionResult> GetLinkedExpenses([FromRoute] string expenseId)
        {
            try
            {
                var expenses = await _expenseService.GetLinkedExpensesAsync(expenseId, User.Identity.Name);
                return Ok(expenses);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom exceptions
            }
        }
    }
}