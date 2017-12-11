using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("login")]
        public IActionResult LogIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/userinfo" });
        }

        [Route("logout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}