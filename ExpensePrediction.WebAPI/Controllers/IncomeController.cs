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
        
        [HttpPost("add")]
        [Authorize("AddIncome")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
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
        
        [HttpGet("{incomeId}", Name = "GetIncome")]
        [Authorize("GetIncome")]
        [Produces(Constants.ApplicationJson)]
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

        [HttpGet]
        [Authorize("GetIncomes")]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetIncomes([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var incomes = await _incomeService.GetIncomesAsync(User.Identity.Name, from, to);

            return Ok(incomes);
        }

        [HttpPost("edit")]
        [Authorize("EditIncome")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
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