using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminTabController : Controller
    {
        private readonly ILogger<AdminDashController> _logger;
        private readonly IGenral _Genral;
        private readonly IAdmin _Admin;
        public AdminTabController(ILogger<AdminDashController> logger, IGenral Genral, IAdmin Admin)
        {
            _logger = logger;
            _Genral = Genral;
            _Admin = Admin;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProviderLoc()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
