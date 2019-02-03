using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        ///     Edits current user's data.
        /// </summary>
        /// <param name="userEditDto">The user data.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("EditUser")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(UserEditDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> EditUser([FromBody] UserEditDto userEditDto)
        {
            var user = await _accountService.EditUserDataAsync(userEditDto, User.Identity.Name);
            return Ok(user);
        }

        /// <summary>
        ///     Gets current user.
        /// </summary>
        /// <returns>Current user data.</returns>
        [HttpGet]
        [Authorize("GetUser")]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(typeof(UserDataDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetUser()
        {
            var user = await _accountService.GetUserDataAsync(User.Identity.Name);
            return Ok(user);
        }

        /// <summary>
        ///     Changes the password.
        /// </summary>
        /// <param name="passwordChangeDto">The data for password change.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("ChangePassword")]
        [Route("change-password")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
        {
            await _accountService.ChangePasswordAsync(passwordChangeDto, User.Identity.Name);
            return Ok();
        }
    }
}