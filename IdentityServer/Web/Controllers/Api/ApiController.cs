using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers.Api
{
    [Route("api")]
    public class ApiController : Controller
    {
        [Route("user")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUser()
        {
            return Ok(new
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });
        }
    }
}