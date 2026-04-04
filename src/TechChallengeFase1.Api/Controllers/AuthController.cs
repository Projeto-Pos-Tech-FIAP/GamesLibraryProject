using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechChallengeFase1.Api.Controllers
{
    [Authorize]
    public class AuthController : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok("login não implementado");
        }
    }
}
