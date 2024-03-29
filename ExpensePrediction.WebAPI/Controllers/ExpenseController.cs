using AutoMapper;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects;
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
        /// <param name="expenseDto">The expense data.</param>
        /// <returns>
        ///     Link to the created expense.
        /// </returns>
        /// <consumes>application/json</consumes>
        [HttpPost("add")]
        [Authorize("AddExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(ExpenseDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
        {
            var expense = await _expenseService.AddExpenseAsync(expenseDto, User.Identity.Name);
            return CreatedAtRoute("GetExpense", new { expenseId = expense.Id }, expense);
        }

        /// <summary>
        ///     Gets data of an expense specified by its id.
        /// </summary>
        /// <param name="expenseId">The expense identifier.</param>
        /// <returns>The expense.</returns>
        [HttpGet("{expenseId}", Name = "GetExpense")]
        [Authorize("GetExpense")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(ExpenseDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetExpense([FromRoute] string expenseId)
        {
            var expense = await _expenseService.GetExpenseAsync(expenseId,
                (await _userManager.FindByIdAsync(User.Identity.Name)).Id);
            return Ok(expense);
        }

        /// <summary>
        ///     Gets user expenses from given period of time.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns>Expenses.</returns>
        [HttpGet]
        [Authorize("GetExpenses")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetExpenses([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var expenses = await _expenseService.GetExpensesAsync(User.Identity.Name, from, to);
            return Ok(expenses);
        }

        /// <summary>
        ///     Edits the expense.
        /// </summary>
        /// <param name="expenseDto">The expense dto.</param>
        /// <returns>Edited expense.</returns>
        [HttpPost("edit")]
        [Authorize("EditExpense")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(ExpenseDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> EditExpense([FromBody] ExpenseDto expenseDto)
        {
            var result = await _expenseService.EditExpenseAsync(expenseDto, User.Identity.Name);
            return Ok(result);
        }

        /// <summary>
        ///     Deletes the expense.
        /// </summary>
        /// <param name="expenseId">The expense id.</param>
        [HttpDelete("{expenseId}")]
        [Authorize("DeleteExpense")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> DeleteExpense([FromRoute]string expenseId)
        {
            await _expenseService.DeleteExpenseAsync(expenseId, User.Identity.Name);
            return NoContent();
        }
    }
}