using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Auth()
        {
            return Ok();
        }
    }
}