using System.Threading.Tasks;
using AutoMapper;
using ExpensePrediction.DataAccessLayer.Entities;
using ExpensePrediction.DataTransferObjects.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpensePrediction.WebAPI.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Edits current user's data.
        /// </summary>
        /// <param name="userEditData">The user data.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Edit([FromBody] UserEditDto userEditData)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FirstName = userEditData.FirstName;
            user.LastName = userEditData.LastName;
            user.PhoneNumber = userEditData.PhoneNumber;

            await _userManager.UpdateAsync(user); //TODO check if saved

            return Ok(_mapper.Map<UserDataDto>(user));
        }

        /// <summary>
        /// Gets current user.
        /// </summary>
        /// <returns>Current user data.</returns>
        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dto = _mapper.Map<UserDataDto>(user);
            return Ok(dto);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="passwordChangeData">The data for password change.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeData)
        {
            if (passwordChangeData.NewPassword != passwordChangeData.NewPasswordRepeated)
            {

                return StatusCode(400, "NO_MATCH"); //TODO Think about different code here
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(user, passwordChangeData.CurrentPassword, passwordChangeData.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(400, "ERROR"); //TODO Return different codes
        }
    }
}