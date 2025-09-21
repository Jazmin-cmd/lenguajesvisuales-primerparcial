using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CooperativaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestAuthController : ControllerBase
    {
        [HttpGet("cualquiera")]
        [Authorize]
        public IActionResult Cualquiera()
        {
            return Ok(new { message = "Accediste con cualquier token válido", user = User.Identity?.Name });
        }

        [HttpGet("solo-socios")]
        [Authorize(Roles = "Socio")]
        public IActionResult SoloSocios()
        {
            return Ok(new { message = "Accediste como Socio", user = User.Identity?.Name });
        }

        [HttpGet("solo-admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult SoloAdmins()
        {
            return Ok(new { message = "Accediste como Admin", user = User.Identity?.Name });
        }
    }
}
