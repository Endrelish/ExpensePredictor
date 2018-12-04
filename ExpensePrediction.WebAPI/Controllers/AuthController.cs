using System;
using System.Threading.Tasks;
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
        /// <returns>
        ///     Access token
        /// </returns>
        [HttpPost("register")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(string), 200)] //TODO Change to TokenDto
        [ProducesResponseType(typeof(string), 400)]
        //TODO Custom exceptions
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var token = await _authService.RegisterAsync(registerDto);
                return Ok(new {Token = token});
            }
            catch (Exception)
            {
                return StatusCode(400, "ERROR"); //TODO Custom exceptions
            }
        }

        /// <summary>
        ///     Gets the token for specified user.
        /// </summary>
        /// <param name="loginDto">The login data.</param>
        /// <returns>
        ///     The token.
        /// </returns>
        [HttpPost("get-token")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(string), 200)] //TODO use TokenDto
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetToken([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.GetTokenAsync(loginDto);
                return Ok(new {Token = token});
            }
            catch (Exception) //TODO custom exception
            {
                return Unauthorized();
            }
        }

        //----------ENDPOINTS BELOW JUST FOR TESTING----------//

        /// <summary>
        ///     Hahas this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet("haha")]
        public IActionResult Haha()
        {
            return Ok(new {Haha = "hehe"});
        }
    }
}