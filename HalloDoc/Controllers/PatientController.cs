using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Models;
using HalloDoc.Repository.Implement;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatient patient;


        public PatientController(ILogger<PatientController> logger, IPatient patient)
        {
            _logger = logger;
            this.patient = patient;
        }
        public IActionResult PatientSite()
        {
            return View();
        }

        public IActionResult SubmitReq()
        {
            ViewBag.ShowAdditionalInput = false;
            return View();
        }

        [HttpGet]
        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        [ActionName("PatientVerification")]
        public IActionResult PatientVerification(Aspnetuser user)
        {
            if (user.Email != null)
            {
                if (patient.CheckExistAspUser(user.Email))
                {
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


        public IActionResult PatientForgotpass()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
