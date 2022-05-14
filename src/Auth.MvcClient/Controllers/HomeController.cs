using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}