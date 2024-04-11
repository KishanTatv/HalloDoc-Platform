using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly ILogger<PhysicianController> _logger;
        private readonly IGenral _Genral;
        private readonly IPhysician _physician;
        private readonly IAdmin _admin;
        private readonly IPatient _patient;

        public PhysicianController(ILogger<PhysicianController> logger, IGenral Genral, IAdmin admin, IPhysician physician, IPatient patient)
        {
            _logger = logger;
            _Genral = Genral;
            _physician = physician;
            _admin = admin;
            _patient = patient;
        }


        #region Accept
        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult AcceptReq(int reqid)
        {
            _Genral.updateReqStatus(reqid, 2);
            return Ok();
        }
        #endregion


        #region Phy Note
        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult ViewNotedata(string note, int reqid)
        {
            if (note != null)
            {
                _admin.addNote(reqid, null, note);
                return RedirectToAction("ViewNotes", "AdminDash", new { reqid = reqid });
            }
            else
            {
                TempData["msg"] = "Please Enter Note!";
                return RedirectToAction("ViewNotes", "AdminDash", new { reqid = reqid });
            }
        }
        #endregion

        #region phy Transfer
        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult TransferPopup(int reqid)
        {
            return PartialView("_PopupTransfer", reqid);
        }

        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult TransferDone(int reqid, string note)
        {
            int phId = _physician.getPhyId(Request.Cookies["CookieEmail"]);
            _Genral.updateReqStatusWithPhysician(reqid, null, 1);
            _Genral.AddreqLogStatus(reqid, note, null, phId, 1);
            return Json(new { value = "Ok" });
        }
        #endregion


        #region Encounter Active
        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult EncounterPopup(int reqid)
        {
            return PartialView("_PopupTypeofCare", reqid);
        }

        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult EncounterProType(int reqid, string type)
        {
            if(type == "Consultant")
            {
                _Genral.updateReqStatus(reqid, 6);
            }
            if(type == "Housecall")
            {
                _Genral.updateReqStatus(reqid, 5);
            }
            return Ok();
        }

        public IActionResult StatusHouseCall(int reqid)
        {
            _Genral.updateReqStatus(reqid, 6);
            return Ok();
        }
        #endregion


        #region Encounter Conclude
        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult ConcludeCare(int reqid)
        {
            if (!_Genral.CheckEncounterFinalize(reqid))
            {
                return Json(new { value = "NotFinalize" });
            }
            else
            {
                var reqFile = _Genral.GetRequestsFileswithReq(reqid);
                return PartialView("_ConcludeCare", reqFile);
            }
        }

        [CustomAuthorize("Admin:Provider", "Dashboard")]
        public IActionResult ConcludeFinal(int reqid, string note)
        {
            if (note != null)
            {
                _admin.addNote(reqid, null, note);
            }
            _Genral.updateReqStatus(reqid, 8);
            return Ok();
        }
        #endregion


        #region myProfile
        [CustomAuthorize("Admin:Provider", "My Profile")]
        public IActionResult Profile()
        {
            int phId = _physician.getPhyId(Request.Cookies["CookieEmail"]);
            PhysicianCustom phinfo = _admin.getPhyProfile(phId);
            var phy = _admin.getPhysicianDetail(phId);
            var region = _admin.getAllRegion();
            var phReg = _physician.phyRegionExist(phId);

            List<string> files = new List<string>();
            files.Add("ContractorAgreement"); files.Add("Background"); files.Add("HIPAA"); files.Add("discloure"); files.Add("License");

            List<string> Doclist = new List<string>();
            foreach (var file in files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "providerDoc", phId + file + ".pdf");
                if (System.IO.File.Exists(filePath))
                {
                    Doclist.Add(filePath);
                }
                else
                {
                    Doclist.Add(null);
                }
            }
            PhysicianProfileViewModel data = new PhysicianProfileViewModel { PhysicianCustom = phinfo, physician = phy, Regions = region, phyReg = phReg, DocFile = Doclist };
            return View(data);
        }
        #endregion
    }
}
