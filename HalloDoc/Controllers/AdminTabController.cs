using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using static HalloDoc.HelperClass.HalloEnum;
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
        private readonly IPatient _patient;
        public AdminTabController(ILogger<AdminDashController> logger, IGenral Genral, IAdmin Admin, IPhysician physician, IPatient patient)
        {
            _logger = logger;
            _Genral = Genral;
            _Admin = Admin;
            _physician = physician;
            _patient = patient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProviderLoc()
        {
            return View();
        }


        #region Provider Location
        public IActionResult ProviderLocData()
        {
            var data = _Admin.getAllPhysicianLocation();
            return Json(new {data});
        }
        #endregion



        #region Admin Profile
        public IActionResult Profile()
        {
            string adminEmail = Request.Cookies["CookieEmail"];
            var admin = _Admin.getAdminProfile(adminEmail);
            var adminCutstom = _Admin.getAdminCustomProfile(adminEmail);
            var region = _Admin.getAllRegion();
            var modelData = new ProfileViewModel { Admin = admin, Regions = region, AdminCustom = adminCutstom };
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
            ModelState["AdminRegions"].ValidationState = ModelValidationState.Valid;
            ModelState["Admin"].ValidationState = ModelValidationState.Valid;
            ModelState["Regions"].ValidationState = ModelValidationState.Valid;
            ModelState["AdminCustom.Email"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
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
                _Admin.updateAdminInfo(adminInfo.AdminCustom, adminEmail);
                return Json(new { value = "changed" });

            }
            else
            {
                return RedirectToAction("Profile");
            }
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
            if (data.Count() == 0)
            {
                TempData["DataMsg"] = "No More Records !";
            }
            ViewBag.CurrentPage = page;
            return PartialView("_ProviderHome", data);
        }

        public IActionResult PhysicanNotification(int phid, string ischeck)
        {
            BitArray bitArray = new BitArray(1);
            if (ischeck == "true")
            {
                bitArray[0] = true;
                _physician.updatePhysicianNotification(phid, bitArray);
            }
            else
            {
                bitArray[0] = false;
                _physician.updatePhysicianNotification(phid, bitArray);
            }
            return Ok();
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
            var phReg = _physician.phyRegionExist(phid);

            List<string> files = new List<string>();
            files.Add("ContractorAgreement"); files.Add("Background"); files.Add("HIPAA"); files.Add("discloure"); files.Add("License");

            List<string> Doclist = new List<string>();
            foreach (var file in files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "providerDoc", phid + file + ".pdf");
                if (System.IO.File.Exists(filePath))
                {
                    Doclist.Add(filePath);
                }
                else
                {
                    Doclist.Add(null);
                }
            }
            var data = new PhysicianProfileViewModel { PhysicianCustom = phinfo, physician = phy, Regions = region, phyReg = phReg, DocFile = Doclist };
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
            var regList = Request.Form["checkReg"].ToList();
            var phReg = _physician.phyRegionExist(phinfo.phid);
            var addReg = regList.Select(int.Parse).Except(phReg).ToList();
            var remReg = phReg.Except(regList.Select(int.Parse)).ToList();
            foreach (var reg in addReg)
            {
                _physician.addPhyRegion(phinfo.phid, reg);
            }
            foreach (var reg in remReg)
            {
                _physician.removeRegion(phinfo.phid, reg);
            }
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
            _physician.updateAdditionPhyData(BusinessN, BusinessW, photoBase64, signatureBase64, Note, phyEmail, aspId);
            return Json(new { value = "changed" });
        }

        public IActionResult Phydocument(int phid, IFormFile ContractorAgreement, IFormFile Background, IFormFile HIPAA, IFormFile discloure, IFormFile License)
        {
            List<IFormFile> files = new List<IFormFile>();
            files.Add(ContractorAgreement); files.Add(Background); files.Add(HIPAA); files.Add(discloure); files.Add(License);
            UploadPhyDocumnet(files, phid);
            BitArray contractBit = new BitArray(1);
            contractBit[0] = false;
            BitArray backgrundBit = new BitArray(1);
            backgrundBit[0] = false;
            BitArray hipaaBit = new BitArray(1);
            hipaaBit[0] = false;
            BitArray discloureBit = new BitArray(1);
            discloureBit[0] = false;
            BitArray licenceBit = new BitArray(1);
            licenceBit[0] = false;

            if (ContractorAgreement != null)
            {
                contractBit[0] = true;
            }
            if (Background != null)
            {
                backgrundBit[0] = true;
            }
            if (HIPAA != null)
            {
                hipaaBit[0] = true;
            }
            if (discloure != null)
            {
                discloureBit[0] = true;
            }
            if (License != null)
            {
                licenceBit[0] = true;
            }
            _physician.AddOnboardingInphysician(phid, contractBit, backgrundBit, hipaaBit, discloureBit, licenceBit);
            return RedirectToAction("Provider");
        }

        public IActionResult DeletePhy(int phid)
        {
            _physician.isDeletePhy(phid);
            return Ok();
        }

        #endregion


        #region createAccount Provider
        public IActionResult NewProvider()
        {
            var region = _Admin.getAllRegion();
            var role = _Admin.getAllroleDetails().Where(x => x.Accounttype == 2).ToList();
            var data = new PhysicianProfileViewModel { Regions = region, Roles = role };
            return PartialView("_CreateNewProvider", data);
        }

        public IActionResult AddNewProvider(PhysicianProfileViewModel model, IFormFile photo, IFormFile ContractorAgreement, IFormFile Background, IFormFile HIPAA, IFormFile discloure, IFormFile License)
        {
            var regList = Request.Form["checkReg"].ToList();
            int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
            byte[] photoBytes;

            using (var memoryStream = new MemoryStream())
            {
                if (photo != null)
                {
                    photo.CopyTo(memoryStream);
                }
                photoBytes = memoryStream.ToArray();
            }
            var photoBase64 = Convert.ToBase64String(photoBytes);
            BitArray contractBit = new BitArray(1);
            contractBit[0] = false;
            BitArray backgrundBit = new BitArray(1);
            backgrundBit[0] = false;
            BitArray hipaaBit = new BitArray(1);
            hipaaBit[0] = false;
            BitArray discloureBit = new BitArray(1);
            discloureBit[0] = false;
            BitArray licenceBit = new BitArray(1);
            licenceBit[0] = false;

            if (ContractorAgreement != null)
            {
                contractBit[0] = true;
            }
            if (Background != null)
            {
                backgrundBit[0] = true;
            }
            if (HIPAA != null)
            {
                hipaaBit[0] = true;
            }
            if (discloure != null)
            {
                discloureBit[0] = true;
            }
            if (License != null)
            {
                licenceBit[0] = true;
            }

            Aspnetuser asp = _physician.CretephyAspnetUser(model.PhysicianCustom);
            _patient.addAspnetUserrole(asp.Id, 2);
            Physician phy = _physician.addNewPhysician(model, photoBase64, aspId, asp.Id);
            _physician.addPhysicianNotification(phy.Physicianid);

            foreach (var reg in regList)
            {
                _physician.addPhyRegion(phy.Physicianid, Convert.ToInt16(reg));
            }
            List<IFormFile> files = new List<IFormFile>();
            files.Add(ContractorAgreement); files.Add(Background); files.Add(HIPAA); files.Add(discloure); files.Add(License);

            UploadPhyDocumnet(files, phy.Physicianid);
            _physician.AddOnboardingInphysician(phy.Physicianid, contractBit, backgrundBit, hipaaBit, discloureBit, licenceBit);
            string body = "Try to login using credentials:\n" + $"Username: {"MD." + model.PhysicianCustom.Lastname.Trim().ToUpper()} {model.PhysicianCustom.Firstname.Trim().Substring(0, 1).ToUpper()}\n" + $"Password: {model.PhysicianCustom.Password}";
            _Genral.SendEmailOffice365(model.PhysicianCustom.Email, "Login Credintial", body, null);
            _Genral.addEmailLog(body, "Login Credintial", model.PhysicianCustom.Email, null, Convert.ToInt16(Request.Cookies["CookieRole"]), null, _Admin.getAdminId(Request.Cookies["CookieEmail"]), null);
            return RedirectToAction("Provider");
        }

        public void UploadPhyDocumnet(List<IFormFile> files, int phyId)
        {
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var format = file.FileName.Split('.')[1];
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/providerDoc/", phyId + file.Name + "." + format);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyToAsync(stream);
                        }
                    }
                }
            }
        }
        #endregion


        #region createAccount Admin
        public IActionResult AdminCreate()
        {
            TempData["SuccMsg"] = TempData["SuccMsg"];

            var role = _Admin.getAllroleDetails().Where(x => x.Accounttype == 1).ToList();
            var reg = _Admin.getAllRegion();
            var data = new adminViewModel { Regions = reg, Roles = role };
            return View(data);
        }


        public IActionResult AddNewAdmin(adminViewModel model)
        {
            if (!_Genral.CheckExistAspUser(model.Email))
            {
                int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
                var regList = Request.Form["checkReg"].ToList();
                Aspnetuser asp = _Admin.CreteAdminAspnetUser(model);
                _patient.addAspnetUserrole(asp.Id, 1);
                Admin admin = _Admin.CreateNewAdmin(model, aspId, asp.Id);
                string body = "Try to login using credentials:\n" + $"Username: {model.Lastname.Trim().ToUpper()} {model.Firstname.Trim().Substring(0, 1).ToUpper()}\n" + $"Password: {model.Password}";
                foreach (var i in regList)
                {
                    _Admin.addAdminRegion(admin.Adminid, Convert.ToInt16(i));
                }
                _Genral.SendEmailOffice365(model.Email, "Login Credintial", body, null);
                _Genral.addEmailLog(body, "Login Credintial", model.Email, null, Convert.ToInt16(Request.Cookies["CookieRole"]), null, _Admin.getAdminId(Request.Cookies["CookieEmail"]), null);

                TempData["SuccMsg"] = "Admin Created with Successfully !";
                return RedirectToAction("AdminCreate");
            }
            else
            {
                TempData["EmailExist"] = "Email is alredy exist";
                var role = _Admin.getAllroleDetails();
                var reg = _Admin.getAllRegion();
                var data = new adminViewModel { Regions = reg, Roles = role };
                return View("AdminCreate", data);
            }
        }
        #endregion


        #region Account Access
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

        #region User Access
        public IActionResult UserAccess()
        {
            var data = _Admin.UserAccessData();
            return View(data);
        }
        #endregion


        #region Scheduling
        public IActionResult Scheduling()
        {
            var region = _Admin.getAllRegion();
            return View(region);
        }

        public IActionResult NewShiftPopUp()
        {
            var region = _Admin.getAllRegion();
            var dataModel = new ShiftPoupViewModel { Regions = region };
            return PartialView("_PopupCreateShft", dataModel);
        }

        public IActionResult checkShiftExist(ShiftPoupViewModel formdata)
        {
            if (!_Admin.checkExistShift(formdata, formdata.shiftdate))
            {
                int shiftDateDay = (int)formdata.shiftdate.DayOfWeek + 1;
                var repeatDayList = Request.Form["checkDays"].ToList();
                foreach (var repeatDateDay in repeatDayList)
                {
                    DateOnly detailDate = new DateOnly();
                    if ((Convert.ToInt32(repeatDateDay) - shiftDateDay) < 0)
                    {
                        detailDate = formdata.shiftdate.AddDays(Convert.ToInt32(repeatDateDay) - shiftDateDay + 7);
                    }
                    else
                    {
                        detailDate = formdata.shiftdate.AddDays(Convert.ToInt32(repeatDateDay) - shiftDateDay);
                    }
                    if (!_Admin.checkExistShift(formdata, detailDate))
                    {
                        for (var i = 0; i < formdata.repeatTime - 1; i++)
                        {
                            DateOnly detailDatesMore = detailDate.AddDays(7);
                            if (!_Admin.checkExistShift(formdata, detailDatesMore))
                            {
                                detailDate = detailDate.AddDays(7);
                            }
                            else
                            {
                                return Json(new { value = "Error" });
                            }
                        }
                    }
                    else
                    {
                        return Json(new { value = "Error" });
                    }
                }
                return Json(new { value = "Ok" });
            }
            else
            {
                return Json(new { value = "Error" });
            }
        }

        public IActionResult shiftData(ShiftPoupViewModel formdata)
        {
            int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
            var repeatDayList = Request.Form["checkDays"].ToList();
            StringBuilder weekDays = new StringBuilder("0000000");
            if (repeatDayList.Contains("Sunday"))
            {
                weekDays[0] = '1';
            }
            if (repeatDayList.Contains("Monday"))
            {
                weekDays[1] = '1';
            }
            if (repeatDayList.Contains("Tuesday"))
            {
                weekDays[2] = '1';
            }
            if (repeatDayList.Contains("Wednesday"))
            {
                weekDays[3] = '1';
            }
            if (repeatDayList.Contains("Thursday"))
            {
                weekDays[4] = '1';
            }
            if (repeatDayList.Contains("Friday"))
            {
                weekDays[5] = '1';
            }
            if (repeatDayList.Contains("Saturday"))
            {
                weekDays[6] = '1';
            }
            string weekday = weekDays.ToString();
            int shiftDateDay = (int)formdata.shiftdate.DayOfWeek + 1;

            Shift shift = _Admin.addNewShift(formdata, weekday, aspId);
            Shiftdetail shiftdetail;
            if (formdata.isRepeat == false)
            {
                shiftdetail = _Admin.addNewShiftDetail(shift.Shiftid, formdata.shiftdate, formdata, 0);
                _Admin.addShiftdetailRegion(shiftdetail.Shiftdetailid, formdata.regId);
            }
            else
            {
                foreach (var repeatDateDay in repeatDayList)
                {
                    DateOnly detailDate = new DateOnly();
                    if ((shiftDateDay - Convert.ToInt32(repeatDateDay)) < 0)
                    {
                        detailDate = formdata.shiftdate.AddDays(shiftDateDay - Convert.ToInt32(repeatDateDay) + 7);
                    }
                    else
                    {
                        detailDate = formdata.shiftdate.AddDays(shiftDateDay - Convert.ToInt32(repeatDateDay));
                    }
                    shiftdetail = _Admin.addNewShiftDetail(shift.Shiftid, detailDate, formdata, 0);
                    _Admin.addShiftdetailRegion(shiftdetail.Shiftdetailid, formdata.regId);
                    for (var i = 0; i < formdata.repeatTime - 1; i++)
                    {
                        DateOnly detailDatesMore = detailDate.AddDays(7);
                        shiftdetail = _Admin.addNewShiftDetail(shift.Shiftid, detailDatesMore, formdata, 0);
                        _Admin.addShiftdetailRegion(shiftdetail.Shiftdetailid, formdata.regId);
                        detailDate = detailDate.AddDays(7);
                    }
                }
            }
            return Json(new { value = "Ok" });
        }

        public IActionResult getPhysicianRecord(int reg)
        {
            var data = _Admin.getAllPhysicianName();
            if (reg != 0)
            {
                data = data.Where(x => x.phyReg.Contains(reg)).ToList();
            }
            return Json(new { phyCustomNameViewModel = data });
        }

        public IActionResult getScheduleData(int reg)
        {
            var data = _Admin.getAllShiftdetail();
            if (reg != 0)
            {
                data = data.Where(x => x.Regionid == reg).ToList();
            }
            return Json(new { Shiftdetail = data });
        }

        public IActionResult viewShift(int eventid)
        {
            var shiftData = _Admin.getShiftDetailSpecific(eventid);
            var region = _Admin.getAllRegion();
            var dataModel = new ShiftPoupViewModel { Regions = region, regId = shiftData.regId, phyid = shiftData.phyid, shiftDetailId = eventid, shiftdate = shiftData.shiftdate, timeStart = shiftData.timeStart, timeEnd = shiftData.timeEnd, shiftStatus = shiftData.shiftStatus };
            return PartialView("_PopupViewShift", dataModel);
        }

        public IActionResult shiftDataEdit(ShiftPoupViewModel formdata)
        {
            if (!_Admin.checkExistShift(formdata, formdata.shiftdate))
            {
                int aspId = _Genral.getAspId(Request.Cookies["CookieEmail"]);
                _Admin.updateShiftDetail(formdata, aspId);
                return Json(new { value = "Ok" });
            }
            else
            {
                return Json(new { value = "Error" });
            }
        }

        public IActionResult retuenShift(int eventid, int status)
        {
            if (status == 1)
            {
                _Admin.changeShiftdetailStatus(eventid, 0);
            }
            else
            {
                _Admin.changeShiftdetailStatus(eventid, 1);
            }
            return Ok();
        }

        public IActionResult deleteShift(int eventid)
        {
            _Admin.deleteShiftDetail(eventid);
            return Ok();
        }

        public IActionResult ShiftReview()
        {
            var region = _Admin.getAllRegion();
            return PartialView("_ShiftReview", region);
        }

        public IActionResult ReviewShiftData(int page, int reg, bool month, List<int> listdeleteItem, List<int> listApproveItem)
        {
            if (listdeleteItem.Count != 0)
            {
                foreach (var item in listdeleteItem)
                {
                    _Admin.deleteShiftDetail(item);
                }
            }
            if (listApproveItem.Count != 0)
            {
                foreach (var item in listApproveItem)
                {
                    _Admin.changeShiftdetailStatus(item, 1);
                }
            }
            var data = _Admin.getAllShiftdetail().Where(x => x.Status == 0).ToList();
            if (reg != 0)
            {
                data = data.Where(x => x.Shiftdetailregions.FirstOrDefault().Regionid == reg).ToList();
            }
            if (month == true)
            {
                data = data.Where(x => x.Shiftdate.Month == System.DateTime.Now.Month).ToList();
            }
            return PartialView("_ShiftReviewData", data);
        }
        #endregion


        #region Schedule Pro On Call
        public IActionResult ProviderCall()
        {
            var region = _Admin.getAllRegion();
            return PartialView("_ProviderOnCall", region);
        }

        public IActionResult proOncallData(int reg)
        {
            IEnumerable<ProOncallModel> dataOnCall = _Admin.phyOncallAvialble(reg).ToList();
            IEnumerable<ProOffcallModel> dataOffCall = _Admin.phyOffcall(reg).ToList();
            var data = new ProviderCallViewModel { ProOncall = dataOnCall, ProOffcall = dataOffCall };
            return PartialView("_ProviderOnCallData", data);
        }
        #endregion


        #region Search Record
        public IActionResult SearchRecord()
        {
            return View();
        }

        public IActionResult searchData(string reqStatus, string reqType, string ptName, DateTime formDate, DateTime toDate, string proName, string phone, string email)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            var data = _Admin.getAllReqData().Where(x => x.Isdeleted != bitArray).ToList();
            return PartialView("_SearchTableData", data);
        }
        #endregion

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
                data = data
                    .Where(x =>
                    (email == null || x.Emailid.ToLower().Contains(email.ToLower())) &&
                    (name == null || (x.Request?.Firstname?.ToLower().Contains(name.ToLower()) ?? false) || (x.Request?.Lastname?.ToLower().Contains(name.ToLower()) ?? false)) &&
                    (crDate == DateTime.MinValue || x.Createdate.Date.Equals(crDate)) &&
                    (cgDate == DateTime.MinValue || x.Sentdate.Equals(cgDate)))
                    .ToList();
                return PartialView("_EmailLog", data);
            }
            else
            {
                return PartialView("_EmailLog");
            }
        }
        #endregion

        #region Patient History
        public IActionResult PatientHistory()
        {
            return View();
        }

        public IActionResult PatientHistoryData(string Hfname, string Hlname, string Hemail, string Hphone)
        {
            var data = _Admin.getAllUserData();
            data = data.Where(x =>
            (Hfname == null || x.Firstname.Contains(Hfname)) &&
            (Hlname == null || x.Lastname.Contains(Hlname)) &&
            (Hemail == null || x.Email.Contains(Hemail)) &&
            (Hphone == null || x.Mobile.Contains(Hphone))
            ).ToList();
            return PartialView("_PatientHistoryData", data);
        }
        #endregion

        #region BlockHistory
        public IActionResult BlockHistory()
        {
            return View();
        }

        public IActionResult BlockData(string name, string email, DateTime date, string phone)
        {
            var data = _Admin.getallBlockRequest();
            data = data.Where(x =>
            (name == null || (x.Request?.Requestclients?.FirstOrDefault().Firstname.ToLower().Contains(name.ToLower()) ?? false) || (x.Request?.Requestclients?.FirstOrDefault().Lastname.ToLower().Contains(name.ToLower()) ?? false)) &&
            (email == null || x.Request.Requestclients.FirstOrDefault().Email.ToLower().Contains(email.ToLower())) &&
            (phone == null || x.Request.Requestclients.FirstOrDefault().Phonenumber.ToLower().Contains(phone))
            ).ToList();
            //if (date != DateTime.MinValue)
            //{
            //    data = data.Where(x => (x.Createddate.ToString().FirstOrDefault().ToString().Contains(date.Date.ToString()))).ToList(); 
            //}
            return PartialView("_BlockTableData", data);
        }

        public IActionResult BlockDataUnblock(int blockreqid)
        {
            return Ok();
        }
        #endregion
    }
}
