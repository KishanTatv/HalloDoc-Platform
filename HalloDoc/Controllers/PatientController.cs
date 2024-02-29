using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Implement;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HalloDoc.Models;
using DocumentFormat.OpenXml.Spreadsheet;

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
        public ActionResult CheckEmail(string email)
        {
            bool emailExists = patient.CheckExistAspUser(email);
            return Json(new { exists = emailExists });
        }


        [HttpPost]
        [ActionName("PatientVerification")]
        public IActionResult PatientVerification(Aspnetuser user)
        {
            if (ModelState.IsValid)
            {
                if (user.Email != null)
                {
                    if (patient.CheckExistAspUser(user.Email))
                    {
                        string userName = patient.userFullName(user.Email);
                        HttpContext.Session.SetString("SessionKeyEmail", user.Email);
                        HttpContext.Session.SetString("SessionKeyName", userName);
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
                string link = Url.Action("NewPassword", "Patient", new { email = userEmail }, Request.Scheme);
                var body = $"Hi,<br /><br />Please click on the following link to change your password credential:<br /><br />" + link;
                patient.sendMail(userEmail, subject, body);
                TempData["Msg"] = "please check your Email";
            }
            else
            {
                TempData["Msg"] = "Invalid Email";
            }
            return RedirectToAction("PatientForgotpass");
        }


        [HttpGet]
        [Route("Patient/NewPassword/{email}")]
        public IActionResult NewPassword(string email)
        {
            return View();
        }



        [HttpPost]
        [Route("Patient/NewPassword/{email}")]
        public IActionResult NewPassword(string email,ClientInformation user)
        {
            if (patient.CheckExistAspUser(email))
            {
                if (user.Password == user.ConfirmPassword)
                {
                    patient.newPasswordCreate(user, email);
                    TempData["Error"] = "Password Changed succesfully!!";
                }
                else
                {
                    TempData["Error"] = "Password and ConfirmPassword not match";
                }
            }
            else
            {
                TempData["Error"] = "Anything Wrong!!";
            }
            return View();
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
                if (patient.CheckExistAspUser(user.Email))
                {
                    TempData["Msg"] = "Alredy Account! Try to Login..";
                    return View();
                }
                else
                {
                    Aspnetuser asp = patient.createonlyAsp(user);
                    patient.updateUserIdWithAsp(asp.Id, asp.Email);
                    TempData["Msg"] = "Your Account created Successfully!!";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("PatientLogin");
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
