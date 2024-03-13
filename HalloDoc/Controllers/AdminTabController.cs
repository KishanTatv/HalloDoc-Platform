using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Admin")]
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

        #region Admin Profile
        public IActionResult Profile()
        {
            string adminEmail = Request.Cookies["CookieEmail"];
            var data = _Admin.getAdminProfile(adminEmail);
            var region = _Admin.getAllRegion();
            var modelData = new ProfileViewModel { Admin = data, Regions = region };
            return View(modelData);
        }

        public IActionResult ResetPass(string pass)
        {
            string adminEmail = Request.Cookies["CookieEmail"];
            _Admin.updatePass(adminEmail, pass);
            return Json(new { value = "ok" }); 
        }

        public IActionResult UpdateAdminProfile(ProfileViewModel adminInfo)
        {
            string adminEmail = Request.Cookies["CookieEmail"];
            _Admin.updateAdminInfo(adminInfo.Admin, adminEmail);
            return Json(new { value = "changed" });
        }

        public IActionResult UpdateAdminLoc(ProfileViewModel adminInfo)
        {
            string adminEmail = Request.Cookies["CookieEmail"];
            _Admin.updateAdminLocation(adminInfo.Admin, adminEmail);
            return Json(new { value = "changed" });
        }
        #endregion

        #region Provider
        public IActionResult Provider()
        {
            return View();
        }
        #endregion
    }
}
