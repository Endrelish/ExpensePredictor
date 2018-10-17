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
    [Route("/api/[controller]")]
    public class ExpenseController : Controller
    {
        private IApplicationRepository<Expense> _expenseRepository;
        private UserManager<User> _userManager;

        public ExpenseController(IApplicationRepository<Expense> expenseRepository, UserManager<User> userManager)
        {
            _expenseRepository = expenseRepository;
            _userManager = userManager;
        }

        [HttpPost("Add-Expense")]
        [Authorize]
        public async Task<IActionResult> AddExpense(ExpenseDto expenseData)
        {
            //TODO create
            await _expenseRepository.SaveAsync(); //TODO catch sth I guess
            var id = "newlyCreatedId";
            var newObject = new object();
            return CreatedAtAction("GetExpense", id, newObject);
        }

        [HttpGet("Get-Expense/{id}", Name = "GetExpense")]
        [Authorize]
        public async Task<IActionResult> GetExpense([FromRoute] string id)
        {
            return Ok(await _expenseRepository.FindByIdAsync(id));
        }

    }
}