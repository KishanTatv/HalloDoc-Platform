using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class PatientDashController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashbord()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
