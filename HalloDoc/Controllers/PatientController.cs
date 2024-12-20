﻿using HalloDoc.Entity.Models;
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
using Microsoft.Net.Http.Headers;
using DocumentFormat.OpenXml.ExtendedProperties;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatient _patient;
        private readonly IGenral _genral;
        private readonly IJwtToken _jwtToken;
        private readonly INotyfService _notyf;

        public PatientController(ILogger<PatientController> logger, IPatient patient, IGenral genral, IJwtToken jwtToken, INotyfService notyf)
        {
            _logger = logger;
            _patient = patient;
            _genral = genral;
            _jwtToken = jwtToken;
            _notyf = notyf;
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


        #region Login Authorization
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
                        if (_genral.CheckAspPassword(user.Email, user.Passwordhash))
                        {
                            string menulist = "Null";
                            string userName = _genral.userFullName(user.Email);
                            Aspnetuser asp = _genral.getUserRole(user.Email);
                            if (asp.Aspnetuserrole.Role.Id != 3)
                            {
                                int roleId = _genral.getroleIdFromEmail(user.Email, asp.Aspnetuserrole.Role.Id);
                                menulist = _genral.getMenulistFromRole(roleId);
                            }
                            var UserToken = _jwtToken.GenrateJwtToken(asp, menulist);

                            Response.Cookies.Append("HalloCookie", UserToken);
                            Response.Cookies.Append("CookieEmail", user.Email);
                            Response.Cookies.Append("CookieUserName", userName);
                            Response.Cookies.Append("CookieRole", asp.Aspnetuserrole.Role.Id.ToString());

                            if (asp.Aspnetuserrole.Role.Name == "Patient")
                            {
                                _notyf.Success("Login Successfully !");
                                return RedirectToAction("Dashbord", "PatientDash");
                            }
                            else if (asp.Aspnetuserrole.Role.Name == "Admin" || asp.Aspnetuserrole.Role.Name == "Provider")
                            {
                                _notyf.Success("Login Successfully !");
                                return RedirectToAction("Dashbord", "AdminDash");
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Password Not Matched";
                        }
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
        #endregion


        #region reset Password
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
        #endregion


        #region New Password
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
                    TempData["msg"] = "Password Changed succesfully!!";
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
        #endregion


        #region CreatePatient
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
                        _patient.updateUserIdWithAsp(asp.Id, asp.Email);
                        _patient.addAspnetUserrole(asp.Id, 3);
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
        #endregion


        #region logout
        public IActionResult logOut()
        {
            Response.Cookies.Delete("HalloCookie");
            return View("PatientLogin");
        }
        #endregion



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
