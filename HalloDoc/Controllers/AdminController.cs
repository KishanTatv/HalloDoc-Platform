
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IPatient patient;
        public AdminController(ILogger<AdminController> logger, IPatient patient)
        {
            _logger = logger;
            this.patient = patient;
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
        public IActionResult verifyAdmin(Aspnetuser user)
        {
            if (user.Email != null)
            {
                if (patient.CheckExistAspUser(user.Email))
                {
                    string userName = patient.userFullName(user.Email);
                    HttpContext.Session.SetString("SessionKeyEmail", user.Email);
                    HttpContext.Session.SetString("SessionKeyClientName", userName);
                    return RedirectToAction("Index", "Home");
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
            return RedirectToAction("PatientLogin");
        }


        public IActionResult ForgotPass()
        {
            return View();
        }
    }
}
