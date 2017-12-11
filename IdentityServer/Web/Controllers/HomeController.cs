using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("userinfo")]
        [Authorize]
        public IActionResult UserInformation()
        {
            return View();
        }

        [Route("spa")]
        public IActionResult Spa()
        {
            return View();
        }
    }
}