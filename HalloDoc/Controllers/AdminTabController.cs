using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using static Uno.WinRTFeatureConfiguration;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Admin")]
    public class AdminTabController : Controller
    {
        private readonly ILogger<AdminDashController> _logger;
        private readonly IGenral _Genral;
        private readonly IAdmin _Admin;
        private readonly IPhysician _physician;
        public AdminTabController(ILogger<AdminDashController> logger, IGenral Genral, IAdmin Admin, IPhysician physician)
        {
            _logger = logger;
            _Genral = Genral;
            _Admin = Admin;
            _physician = physician;
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

        public IActionResult adminReg(int adId)
        {
            var data = _Admin.getAdminReg(adId);
            return Json(new { Adminregion = data });
        }

        public IActionResult ResetPass(string pass)
        {
            string adminEmail = Request.Cookies["CookieEmail"];
            _Admin.updatePass(adminEmail, pass);
            return Json(new { value = "ok" });
        }

        public IActionResult UpdateAdminProfile(ProfileViewModel adminInfo)
        {
            var regList = Request.Form["checkReg"].ToList();
            string adminEmail = Request.Cookies["CookieEmail"];
            int adminId = _Admin.getAdminId(adminEmail);
            List<int> r = new List<int>();
            foreach (var reg in _Admin.getAdminReg(adminId))
            {
                r.Add(reg.Region.Regionid);
            }

            foreach (var i in regList)
            {
                if (!_Admin.CheckAdminReg(adminId, Convert.ToInt16(i)))
                {
                    _Admin.addAdminRegion(adminId, Convert.ToInt16(i));
                }
            }
            foreach (var f in r)
            {
                if (!regList.Contains(f.ToString()))
                {
                    _Admin.removeAdminReg(adminId, f);
                }
            }
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
            var data = _Admin.getAllPhysicianData().ToList();
            return View(data);
        }

        public IActionResult PcontactPop(int phid)
        {
            TempData["phid"] = phid;
            return PartialView("PopupProvidercontact");
        }

        public IActionResult Pcontactsend(int phid, string contType, string note)
        {
            return View();
        }

        public IActionResult PhysicanEdit(int phid)
        {
            TempData["phid"] = phid;
            PhysicianCustom phinfo = _Admin.getPhyProfile(phid);
            var region = _Admin.getAllRegion();
            var data = new PhysicianProfileViewModel { PhysicianCustom = phinfo, Regions = region };
            return PartialView("_ProPhysicianEdit", data);
        }

        public IActionResult ResetPhyPass(string pass, int phid)
        {
            string phyEmail = _Admin.getPhysicianEmail(phid);
            _Admin.updatePass(phyEmail, pass);
            return Json(new { value = "ok" });
        }

        public IActionResult UpdatePhyProfile(PhysicianProfileViewModel phinfo)
        {
            int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
            string phyEmail = _Admin.getPhysicianEmail(phinfo.phid);
            _physician.updatePhysicianInfo(phinfo.physician, phyEmail, aspId);
            return Json(new { value = "changed" });
        }

        public IActionResult UpdatePhyLoc(PhysicianProfileViewModel phinfo)
        {
            int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
            string phyEmail = _Admin.getPhysicianEmail(phinfo.phid);
            _physician.updatePhysicianLocation(phinfo.physician, phyEmail, aspId);
            return Json(new { value = "changed" });
        }

        public IActionResult UpdatePhyAdditional(string BusinessN, string BusinessW, string Note, int phid, IFormFile Photo, IFormFile Sign)
        {

            int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
            string phyEmail = _Admin.getPhysicianEmail(phid);

            byte[] photoBytes;
            byte[] signatureBytes;

            using (var memoryStream = new MemoryStream())
            {
                if (Photo != null)
                {
                    Photo.CopyTo(memoryStream);
                }
                photoBytes = memoryStream.ToArray();
            }
            var photoBase64 = Convert.ToBase64String(photoBytes);

            using (var memoryStream = new MemoryStream())
            {
                if (Sign != null)
                {
                    Sign.CopyTo(memoryStream);
                }
                signatureBytes = memoryStream.ToArray();
            }
            var signatureBase64 = Convert.ToBase64String(signatureBytes);
            _physician.updateAdditionPhyData(BusinessW, BusinessW, photoBase64, signatureBase64, phyEmail, aspId);
            return Json(new { value = "changed" });
        }

        #endregion
    }
}
