using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
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
        [ProducesResponseType(typeof(TokenDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var token = await _authService.RegisterAsync(registerDto);
            return Ok(token);
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
        [ProducesResponseType(typeof(TokenDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetToken([FromBody] LoginDto loginDto)
        {
            var token = await _authService.GetTokenAsync(loginDto);
            return Ok(token);
        }

        //----------ENDPOINTS BELOW JUST FOR TESTING----------//

        /// <summary>
        ///     Hahas this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet("haha")]
        public IActionResult Haha()
        {
            return Ok(new { Haha = "hehe", Hehe = "haha", Hoho = "huhu", Huhu = "hoho" });
        }
    }
}
