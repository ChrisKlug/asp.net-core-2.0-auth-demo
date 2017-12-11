using Web.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("login")]
        public IActionResult LogIn()
        {
            return View(new LogInModel());
        }

        [Route("login")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userService.Authenticate(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("InvalidCredentials", "Could not validate your credentials");
                return View(model);
            }

            return await SignInUser(user);
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userService.Add(model.Name, model.Email, model.Password);

            return await SignInUser(user);
        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("logout")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("loginexternal/{id}")]
        public Task LogInExternal(string id) {
            //return HttpContext.ChallengeAsync(id, new AuthenticationProperties { RedirectUri = "/userinfo" });
            return HttpContext.ChallengeAsync(id, new AuthenticationProperties { RedirectUri = "/auth/registerexternal" });
        }

        [Route("registerexternal")]
        public async Task<IActionResult> RegisterExternal(string authprovider)
        {
            var authResult = await HttpContext.AuthenticateAsync("TempCookie");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userService.AuthenticateExternal(authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier));
            if (user != null)
            {
                return await SignInExternal(user);
            }

            return View(new RegisterExternalModel {
                Name = authResult.Principal.FindFirstValue(ClaimTypes.Name),
                Email = authResult.Principal.FindFirstValue(ClaimTypes.Email)
            });
        }

        [Route("registerexternal/{id}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RegisterExternal(string id, RegisterExternalModel model)
        {
            var authResult = await HttpContext.AuthenticateAsync("TempCookie");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userService.AddExternal(authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier), model.Name, model.Email);
            
            return await SignInExternal(user);
        }

        private async Task<IActionResult> SignInExternal(User user)
        {
            await HttpContext.SignOutAsync("TempCookie");
            return await SignInUser(user);
        }
        private async Task<IActionResult> SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("UserInformation", "Home");
        }
    }
}