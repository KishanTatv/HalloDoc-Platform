using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Admin:Provider")]
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
        public IActionResult AcceptReq(int reqid)
        {
            _Genral.updateReqStatus(reqid, 2);
            return Ok();
        }
        #endregion

        #region Phy Note
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
        public IActionResult TransferPopup(int reqid)
        {
            return PartialView("_PopupTransfer", reqid);
        }


        public IActionResult TransferDone(int reqid, string note)
        {
            int phId = _physician.getPhyId(Request.Cookies["CookieEmail"]);
            _Genral.updateReqStatusWithPhysician(reqid, null, 1);
            _Genral.AddreqLogStatus(reqid, note, null, phId, 1);
            return Json(new { value = "Ok" });
        }
        #endregion


        #region Encounter Active
        public IActionResult EncounterPopup(int reqid)
        {
            return PartialView("_PopupTypeofCare", reqid);
        }

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
        public IActionResult ConcludeCare(int reqid)
        {
            if (!_Genral.CheckEncounterFinalize(reqid))
            {
                return Json(new { value = "NotFinalize" });
            }
            else
            {
                return Json(new { value = "Ok" });
            }
        }
        #endregion
    }
}
