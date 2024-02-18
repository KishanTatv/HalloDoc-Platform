using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Models;
using HalloDoc.Repository.Implement;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

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

        public enum Status
        {
            Unassigned = 1, Accepted, Cancelled, Reserving, MDEnRoute, MDOnSite, FollowUp, Closed, Locked, Declined, Consult, Clear, CancelledByProvider, CCUploadedByClient, CCApprovedByAdmin
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
                    string userName = patient.userFullName(user);
                    HttpContext.Session.SetString("SessionKeyEmail", user.Email);
                    HttpContext.Session.SetString("SessionKeyClientName", userName);
                    return RedirectToAction("Dashbord", "PatientDash");
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

        [HttpPost]
        public IActionResult ResetPassEmail(Aspnetuser user)
        {
            if (patient.CheckExistAspUser(user.Email))
            {
                var userEmail = user.Email;
                var subject = "Password reset request";
                string link = Url.Action("NewPassword", "Patient", new { email = user.Email }, Request.Scheme);
                var body = $"Hi,<br /><br />Please click on the following link to create your account:<br /><br />https://localhost:44372/" + link;
                patient.sendMailResetPassword(user, subject, body);
                TempData["Msg"] = "please check your Email";
            }
            else
            {
                TempData["Msg"] = "Invalid Email";
            }
            return RedirectToAction("PatientForgotpass");
        }

        public IActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewPassword(ClientInformation user, string email)
        {
            if(user.Password == user.ConfirmPassword)
            {
                patient.newPasswordCreate(user, email);
            }
            else
            {
                TempData["Error"] = "Password and ConfirmPassword not match";
            }
            return RedirectToAction("PatientLogin");
        }


        [HttpGet]
        public IActionResult CreatePatient()
        {
            return View();
        }


        [HttpPost]
        [ActionName("CreatePatient")]
        public IActionResult CreatePatient(Aspnetuser user)
        {
            if (ModelState.IsValid)
            {
                patient.createPatient(user);
                return RedirectToAction("PatientLogin");
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }


        public IActionResult logOut()
        {
            HttpContext.Session.Clear();
            return View("PatientLogin");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
