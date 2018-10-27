using System.Threading.Tasks;
using AuthWebApi.Data;
using AuthWebApi.Data.Users;
using AuthWebApi.Data.Users.Entities;
using AuthWebApi.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Edits current user's data.
        /// </summary>
        /// <param name="userEditData">The user data.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] UserEditDto userEditData)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FirstName = userEditData.FirstName;
            user.LastName = userEditData.LastName;
            user.PhoneNumber = userEditData.PhoneNumber;

            await _context.SaveChangesAsync(); //TODO check if saved

            return Ok();
        }

        /// <summary>
        /// Gets current user.
        /// </summary>
        /// <returns>Current user data.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(_mapper.Map<User, UserDataDto>(await _userManager.FindByNameAsync(User.Identity.Name)));
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="passwordChangeData">The data for password change.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-password")]
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