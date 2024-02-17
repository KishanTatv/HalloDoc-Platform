using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        public IActionResult AdminForgotPass()
        {
            return View();
        }
    }
}
