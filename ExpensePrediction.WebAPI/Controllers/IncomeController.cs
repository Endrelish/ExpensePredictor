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
    [Route("/api/income")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public IncomeController(UserManager<User> userManager,
            IMapper mapper, IIncomeService incomeService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _incomeService = incomeService;
        }

        /// <summary>
        ///     Adds the income.
        /// </summary>
        /// <param name="incomeDto">The income dto.</param>
        /// <returns>Link to the created income.</returns>
        [HttpPost("add")]
        [Authorize("AddIncome")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IncomeDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> AddIncome([FromBody] IncomeDto incomeDto)
        {
            try
            {
                var income = await _incomeService.AddIncomeAsync(incomeDto, User.Identity.Name);
                return CreatedAtRoute("GetIncome", new {incomeId = income.Id}, income);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom error codes and exceptions
            }
        }

        /// <summary>
        ///     Gets the income.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        /// <returns>The income.</returns>
        [HttpGet("{incomeId}", Name = "GetIncome")]
        [Authorize("GetIncome")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IncomeDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> GetIncome([FromRoute] string incomeId)
        {
            try
            {
                var income = await _incomeService.GetIncomeAsync(incomeId,
                    (await _userManager.FindByIdAsync(User.Identity.Name)).Id);
                return Ok(income);
            }
            catch (Exception) //TODO Custom exceptions
            {
                return StatusCode(400, "ERROR"); //TODO Custom error codes
            }
        }

        /// <summary>
        ///     Gets the incomes.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns>The incomes.</returns>
        [HttpGet]
        [Authorize("GetIncomes")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<IncomeDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> GetIncomes([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var incomes = await _incomeService.GetIncomesAsync(User.Identity.Name, from, to);
                return Ok(incomes);
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR"); //TODO Custom exceptions
            }
        }

        /// <summary>
        ///     Edits the income.
        /// </summary>
        /// <param name="incomeDto">The income dto.</param>
        /// <returns>The edited income</returns>
        [HttpPost("edit")]
        [Authorize("EditIncome")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<IncomeDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> EditIncome([FromBody] IncomeDto incomeDto)
        {
            try
            {
                var result = await _incomeService.EditIncomeAsync(incomeDto, User.Identity.Name);
                return Ok(result);
            }
            catch (Exception e) //TODO Custom exceptions
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}