using System.Threading.Tasks;
using AuthWebApi.Data;
using AuthWebApi.Data.Entities.Expenses;
using AuthWebApi.Data.Users.Entities;
using AuthWebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    [Route("/api/expense")]
    public class ExpenseController : Controller
    {
        private IApplicationRepository<Expense> _expenseRepository;
        private UserManager<User> _userManager;

        public ExpenseController(IApplicationRepository<Expense> expenseRepository, UserManager<User> userManager)
        {
            _expenseRepository = expenseRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Adds an expense.
        /// </summary>
        /// <consumes>application/json</consumes>
        /// <param name="expenseData">The expense data.</param>
        /// <returns>Link to the created expense</returns>
        [HttpPost("add-expense")]
        [Authorize]
        public async Task<IActionResult> AddExpense([FromBody]ExpenseDto expenseData)
        {
            //TODO create
            await _expenseRepository.SaveAsync(); //TODO catch sth I guess
            var id = "newlyCreatedId";
            var newObject = new object();
            return CreatedAtAction("GetExpense", id, newObject);
        }

        /// <summary>
        /// Gets the expense data.
        /// </summary>
        /// <param name="id">The expense identifier.</param>
        /// <returns></returns>
        [HttpGet("get-expense/{id}", Name = "GetExpense")]
        [Authorize]
        public async Task<IActionResult> GetExpense([FromRoute] string expenseId)
        {
            return Ok(await _expenseRepository.FindByIdAsync(expenseId));
        }

    }
}