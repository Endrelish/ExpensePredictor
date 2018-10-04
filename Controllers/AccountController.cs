using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AuthWebApi.Data;
using AuthWebApi.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;
        private IMapper _mapper;

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

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(_mapper.Map<User, UserDataDto>(await _userManager.FindByNameAsync(User.Identity.Name)));
        }
    }
}