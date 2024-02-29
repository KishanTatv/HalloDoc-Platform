
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdmin _admin;
        public AdminController(ILogger<AdminController> logger, IAdmin admin)
        {
            _logger = logger;
            this._admin = admin;
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
        public IActionResult verifyAdmin(Admin admin)
        {
            if (admin.Email != null)
            {
                if (_admin.CheckExistAdmin(admin.Email))
                {
                    string userName = _admin.getAdminUserName(admin.Email);
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
