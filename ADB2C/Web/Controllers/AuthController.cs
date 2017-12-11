using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("login")]
        public IActionResult LogIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/userinfo" }, "B2C_1_sign_in");
        }

        [Route("logout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync();
            var authscheme = User.FindFirstValue("tfp");
            await HttpContext.SignOutAsync(authscheme);
        }

        [Route("register")]
        public IActionResult Register()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/userinfo" }, "B2C_1_sign_up");
        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}