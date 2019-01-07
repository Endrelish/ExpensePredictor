using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/prediction")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }
        [HttpPost]
        [Authorize("Prediction")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> Prediction([FromBody] ExpenseDto expenseDto)
        {
            return Ok(await _predictionService.Prediction(expenseDto, User.Identity.Name));
        }
    }
}