using System.Threading.Tasks;
using AuthWebApi.Data;
using AuthWebApi.Data.Users;
using AuthWebApi.Data.Users.Entities;
using AuthWebApi.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    [Route("[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UserEditDto dto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync(); //TODO check if saved

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(_mapper.Map<User, UserDataDto>(await _userManager.FindByNameAsync(User.Identity.Name)));
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto dto)
        {
            if (dto.NewPassword != dto.NewPasswordRepeated)
            {

                return StatusCode(400, "NO_MATCH"); //TODO Think about different code here
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(400, "ERROR"); //TODO Return different codes
        }
    }
}