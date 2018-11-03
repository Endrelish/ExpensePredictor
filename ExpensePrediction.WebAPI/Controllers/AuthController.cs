using System;
using System.Threading.Tasks;
using AuthWebApi.Dto;
using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        ///     Registers a new user with specified data.
        /// </summary>
        /// <param name="registerDto">The registration data.</param>
        /// <returns></returns>
        [HttpPost("register")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var token = await _authService.Register(registerDto);
                return Ok(new {Token = token});
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR"); //TODO custom error codes
            }
        }

        /// <summary>
        ///     Gets the token for specified user.
        /// </summary>
        /// <param name="loginDto">The login data.</param>
        /// <returns>The token.</returns>
        [HttpPost("get-token")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetToken([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.GetToken(loginDto);
                return Ok(new {Token = token});
            }
            catch (Exception) //TODO custom exception
            {
                return Unauthorized();
            }
        }
    }
}