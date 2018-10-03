using System.IdentityModel.Tokens.Jwt;
using AuthWebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public IActionResult Edit([FromBody] UserDto dto)
        {
            var name = User.Identity.Name;
            return Ok();
        }

        [HttpGet]
        [ActionName("Get-User")]
        [Authorize]
        public IActionResult GetUser()
        {
            return Ok(User.Identity.Name);
        }
    }
}