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
using HalloDoc.Repository.Service.Interface;
using HalloDoc.Repository.Service;
using Windows.Web.Http;
using Microsoft.Net.Http.Headers;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatient _patient;
        private readonly IGenral _genral;
        private readonly IJwtToken _jwtToken;

        public PatientController(ILogger<PatientController> logger, IPatient patient, IGenral genral, IJwtToken jwtToken)
        {
            _logger = logger;
            _patient = patient;
            _genral = genral;
            _jwtToken = jwtToken;
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
            bool emailExists = _genral.CheckExistAspUser(email);
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
                    if (_genral.CheckExistAspUser(user.Email))
                    {
                        //if (user.Passwordhash.GetHashCode().ToString() == patient.CheckAspPassword(user.Email))
                        //{
                        string userName = _genral.userFullName(user.Email);
                        Aspnetuser asp = _genral.getUserRole(user.Email);

                        Console.WriteLine(user.Passwordhash.GetHashCode().ToString());
                        var UserToken = _jwtToken.GenrateJwtToken(asp);
                        Response.Cookies.Append("HalloCookie", UserToken);
                        Response.Cookies.Append("CookieEmail", user.Email);
                        Response.Cookies.Append("CookieUserName", userName);
                        Response.Cookies.Append("CookieRole", asp.Aspnetuserrole.Role.Name);

                        if (asp.Aspnetuserrole.Role.Name == "Patient")
                        {
                            return RedirectToAction("Dashbord", "PatientDash");
                        }
                        else if(asp.Aspnetuserrole.Role.Name == "Admin")
                        {
                            return RedirectToAction("Dashbord", "AdminDash");
                        }


                        //}
                        //else
                        //{
                        //    TempData["Error"] = "Password Not Matched";
                        //}
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
            if (_genral.CheckExistAspUser(user.Email))
            {
                var userEmail = user.Email;
                var subject = "Password reset request";
                string link = Url.Action("NewPassword", "Patient", new { email = userEmail }, Request.Scheme);
                var body = $"Hi,<br /><br />Please click on the following link to change your password credential:<br /><br />" + link;
                _patient.sendMail(userEmail, subject, body);
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
        public IActionResult NewPassword(string email, ClientInformation user)
        {
            if (_genral.CheckExistAspUser(email))
            {
                if (user.Password == user.ConfirmPassword)
                {
                    _patient.newPasswordCreate(user, email);
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
        public IActionResult CreatePatient(ClientInformation user)
        {
            if (ModelState.IsValid)
            {
                if (_genral.CheckExistAspUser(user.Email))
                {
                    TempData["Msg"] = "Alredy Account! Try to Login..";
                    return View();
                }
                else
                {
                    if (user.Password == user.ConfirmPassword)
                    {
                        Aspnetuser asp = _patient.createonlyAsp(user);
                        _patient.AddAspnetUserRole(asp.Id);
                        _patient.updateUserIdWithAsp(asp.Id, asp.Email);
                        TempData["Msg"] = "Your Account created Successfully!!";
                        return View();
                    }
                    else
                    {
                        TempData["Msg"] = "Password not matched!";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("PatientLogin");
            }
        }


        public IActionResult logOut()
        {
            Response.Cookies.Delete("HalloCookie");
            return View("PatientLogin");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
