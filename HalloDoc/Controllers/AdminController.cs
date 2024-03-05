
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdmin _admin;
        private readonly IGenral _genral;
        public AdminController(ILogger<AdminController> logger, IAdmin admin, IGenral genral)
        {
            _logger = logger;
            _admin = admin;
            _genral = genral;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ActionName("verifyAdmin")]
        public IActionResult verifyAdmin(Aspnetuser admin)
        {
            if (admin.Email != null)
            {
                if (_admin.CheckExistAdmin(admin.Email))
                {
                    string userName = _genral.userFullName(admin.Email);
                    int AdminId = _admin.getAdminId(admin.Email);
                    HttpContext.Session.SetString("SessionKeyEmail", admin.Email);
                    HttpContext.Session.SetString("SessionKeyName", userName);
                    HttpContext.Session.SetInt32("SessionKeyAdminId", AdminId);
                    return RedirectToAction("Dashbord", "AdminDash");
                }
                else
                {
                    TempData["Error"] = "Email Not Valid";
                }
            }
            else
            {
                TempData["Error"] = "Email Required";
            }
            return RedirectToAction("Login", "Admin");
        }


        public IActionResult ForgotPass()
        {
            return View();
        }
    }
}
