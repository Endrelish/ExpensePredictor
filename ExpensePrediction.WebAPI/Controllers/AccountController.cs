using ExpensePrediction.BusinessLogicLayer.Interfaces.Services;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Edits current user's data.
        /// </summary>
        /// <param name="userEditDto">The user data.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> Edit([FromBody] UserEditDto userEditDto)
        {
            try
            {
                var user = await _accountService.EditUserData(userEditDto, User.Identity.Name);
                return Ok(user);
            }
            catch (System.Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        /// <summary>
        /// Gets current user.
        /// </summary>
        /// <returns>Current user data.</returns>
        [HttpGet]
        [Authorize]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _accountService.GetUserData(User.Identity.Name);
                return Ok(user);
            }
            catch (System.Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom exceptions
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="passwordChangeDto">The data for password change.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
        [Consumes(Constants.ApplicationJson)]
        [Produces(Constants.ApplicationJson)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
        {
            try
            {
                await _accountService.ChangePassword(passwordChangeDto, User.Identity.Name);
                return Ok();
            }
            catch (System.Exception e)
            {
                return StatusCode(400, e.Message); //TODO custom exceptions
            }
        }
    }
}