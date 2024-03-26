using DocumentFormat.OpenXml.Wordprocessing;
using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        public IActionResult Provider(int page)
        {
            var reg = _Admin.getAllRegion();
            return View(reg);
        }

        public IActionResult ProviderData(int page, int reg)
        {
            var data = _Admin.getAllPhysicianData().ToList();
            ViewBag.TPage = Math.Ceiling(data.Count() / 5.0);

            if (reg != 0)
            {
                data = data.Where(x => x.Regionid == reg).ToList();
            }
            var Tcount = data.Count();
            data = data.Skip(page * 5).Take(5).ToList();
            ViewBag.CurrentPage = page;
            return PartialView("_ProviderHome", data);
        }

        public IActionResult PcontactPop(int phid)
        {
            TempData["phid"] = phid;
            return PartialView("PopupProvidercontact");
        }

        public IActionResult Pcontactsend(int phid, string contType, string note)
        {
            int adminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            int roleId = Convert.ToInt16(Request.Cookies["CookieRole"]);
            string phyEmail = _Admin.getPhysicianEmail(phid);
            string sub = "Communication Notification";
            if (contType == "Email" || contType == "Both")
            {
                _Genral.SendEmailOffice365(phyEmail, sub, note, null);
                _Genral.addEmailLog(note, sub, phyEmail, null, roleId, null, adminId, null);
            }
            return Ok();
        }

        public IActionResult PhysicanEdit(int phid)
        {
            TempData["phid"] = phid;
            PhysicianCustom phinfo = _Admin.getPhyProfile(phid);
            var phy = _Admin.getPhysicianDetail(phid);
            var region = _Admin.getAllRegion();
            var data = new PhysicianProfileViewModel { PhysicianCustom = phinfo, physician = phy, Regions = region };
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
            _physician.updatePhysicianInfo(phinfo.PhysicianCustom, phyEmail, aspId);
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

        public IActionResult DeletePhy(int phid)
        {
            _physician.isDeletePhy(phid);
            return Ok();
        }

        #endregion


        #region createAccount
        public IActionResult NewProvider()
        {
            var region = _Admin.getAllRegion();
            var data = new PhysicianProfileViewModel { Regions = region };
            return PartialView("_CreateNewProvider", data);
        }

        public IActionResult AddNewProvider(PhysicianProfileViewModel model, IFormFile photo)
        {
            _physician.addNewPhysician(model);
            return Ok();
        }
        #endregion


        #region Access
        public IActionResult Access()
        {
            var data = _Admin.getAllroleDetails();
            return View(data);
        }

        public IActionResult CreateRole()
        {
            var role = _Admin.getAllAspnetrole();
            var menu = _Genral.getMenuNames(0);
            var data = new CreateRoleViewModel { aspnetrole = role, Menus = menu };
            return PartialView("_CreateRole", data);
        }

        public IActionResult EditRole(int roleid)
        {
            var role = _Admin.getAllAspnetrole();
            var menu = _Genral.getMenuNames(0);
            var roleinfo = _Genral.getRoleinfo(roleid);
            var roleMenu = _Genral.getAllroleMenu(roleid);
            var data = new CreateRoleViewModel { aspnetrole = role, Menus = menu, Role = roleinfo, rolemenus = roleMenu };
            return PartialView("_CreateRole", data);
        }

        public IActionResult DeletePopup(int roleid)
        {
            TempData["roleId"] = roleid;
            return PartialView("PopupDeleteRole");
        }

        public IActionResult deleteRole(int roleid)
        {
            _Genral.deleteRole(roleid);
            return Ok();
        }

        public IActionResult AccOption(int AccType, int roleid)
        {
            var menu = _Genral.getMenuNames(AccType);
            var roleMenu = _Genral.getAllroleMenu(roleid);
            var data = new RoleMenuCheckbox { menus = menu, rolemenus = roleMenu };
            return PartialView("_MenuOption", data);
        }

        public IActionResult SaveRole(int roleid, string RoleName, short AccType, List<int> Pages)
        {
            int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
            if (RoleName == null || AccType == 0 || Pages.Count() == 0)
            {
                return Json(new { value = "error" });
            }
            else if (roleid != 0)
            {
                _Admin.UpdateRole(roleid, RoleName, AccType, aspId);
                var roleMenu = _Genral.getAllroleMenu(roleid);
                foreach (var item in Pages)
                {
                    if (!_Admin.checkRoleMenu(roleid, item))
                    {
                        _Admin.addRoleMenu(roleid, item);
                    }
                }
                foreach (var role in roleMenu)
                {
                    if (!Pages.Contains(role.Menuid))
                    {
                        _Admin.removeRoleMenu(roleid, role.Menuid);
                    }
                }
                return Json(new { value = "done" });
            }
            else
            {
                Role role = _Admin.AddRole(RoleName, AccType, aspId);
                foreach (var item in Pages)
                {
                    _Admin.addRoleMenu(role.Roleid, item);
                }
                return Json(new { value = "done" });
            }
        }
        #endregion


        public IActionResult SearchRecord()
        {
            return View();
        }

        #region EmailSMS Log
        public IActionResult SMSlog()
        {
            ViewBag.logType = "SMS";
            return View("EmailSMSlog");
        }

        public IActionResult Emaillog(string email)
        {
            ViewBag.logType = "Email";
            return View("EmailSMSlog");
        }

        public IActionResult EmailSMSlogData(string email, string name, DateTime crDate, DateTime cgDate, string checklog)
        {
            ViewBag.logType = checklog;
            if (checklog == "Email")
            {
                var data = _Admin.getEmailLogData();
                if (email != null)
                {
                    data = data.Where(x => x.Emailid.ToLower().Contains(email.ToLower())).ToList();
                }
                if (name != null)
                {
                    data = data.Where(x => (x.Request?.Firstname?.ToLower().Contains(name.ToLower()) ?? false) || (x.Request?.Lastname?.ToLower().Contains(name.ToLower()) ?? false)).ToList();
                }
                if (crDate != DateTime.MinValue)
                {
                    data = data.Where(x => x.Createdate.Date.Equals(crDate)).ToList();
                }
                if (cgDate != DateTime.MinValue)
                {
                    data = data.Where(x => x.Sentdate.Equals(cgDate)).ToList();
                }
                return PartialView("_EmailLog", data);
            }
            else
            {
                return PartialView("_EmailLog");
            }
        }
        #endregion


        public IActionResult PatientHistory()
        {
            return View();
        }
        public IActionResult BlockHistory()
        {
            return View();
        }

        public IActionResult BlockData(string name, string email, DateTime date, string phone)
        {
            var data = _Admin.getallBlockRequest();
            if (name != null)
            {
                data = data.Where(x => (x.Request?.Requestclients?.FirstOrDefault().Firstname.ToLower().Contains(name.ToLower()) ?? false) || (x.Request?.Requestclients?.FirstOrDefault().Firstname.ToLower().Contains(name.ToLower()) ?? false)).ToList();
            }
            //if (email != null)
            //{
            //    data = data.Where(x => (x.Request?.Requestclients?.FirstOrDefault().Email.ToLower().Contains(email.ToLower());
            //}
            //if (crDate != DateTime.MinValue)
            //{
            //    data = data.Where(x => x.Createdate.Date.Equals(crDate)).ToList();
            //}
            //if (cgDate != DateTime.MinValue)
            //{
            //    data = data.Where(x => x.Sentdate.Equals(cgDate)).ToList();
            //}
            return PartialView("_BlockTableData", data);
        }
    }
}
