
using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using HalloDoc.Repository.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdmin _admin;
        private readonly IGenral _genral;
        private readonly IJwtToken _jwtToken;
        private readonly INotyfService _notyf;

        public AdminController(ILogger<AdminController> logger, IAdmin admin, IGenral genral, IJwtToken jwtToken, INotyfService notyf)
        {
            _logger = logger;
            _admin = admin;
            _genral = genral;
            _jwtToken = jwtToken;
            _notyf = notyf;
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
            return RedirectToAction("Login");
        }


        public IActionResult ForgotPass()
        {
            return View();
        }
    }
}
