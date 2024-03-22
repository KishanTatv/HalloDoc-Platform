using ClosedXML.Excel;
using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Admin")]
    public class AdminDashController : Controller
    {
        private readonly ILogger<AdminDashController> _logger;
        private readonly IGenral _Genral;
        private readonly IAdmin _Admin;
        private readonly IPatient _Patient;
        public AdminDashController(ILogger<AdminDashController> logger, IGenral _Genral, IAdmin _Admin, IPatient _Patient)
        {
            _logger = logger;
            this._Genral = _Genral;
            this._Admin = _Admin;
            this._Patient = _Patient;
        }


        public IActionResult Index()
        {
            return View();
        }


        #region Main Dashbord 
        public IActionResult Dashbord()
        {
            var Tcount = _Admin.TotalCountPatient();
            var region = _Admin.getAllRegion();

            var dashData = new DashTable { ToatlCount = Tcount, Regions = region };
            return View(dashData);
        }


        public IActionResult DashbordData(int id, int page, string search, string reg, int reqtype)
        {
            var Tcount = _Admin.TotalCountPatient();
            int pageSize = 5;

            DashTable Req = new DashTable();
            List<short> status = new List<short>();

            switch (id)
            {
                case 1:   //New
                    ViewBag.TPage = Math.Ceiling(Tcount[0] / 5.0);
                    ViewBag.dTable = id;
                    status.Add(1);
                    Req = _Admin.GetPartialTableData(status, page, pageSize, search, reg, reqtype);
                    break;
                case 2:   //Pending
                    ViewBag.TPage = Math.Ceiling(Tcount[1] / 5.0);
                    ViewBag.dTable = id;
                    status.Add(2);
                    Req = _Admin.GetPartialTableData(status, page, pageSize, search, reg, reqtype);
                    break;
                case 3:   //Active
                    ViewBag.TPage = Math.Ceiling(Tcount[2] / 5.0);
                    ViewBag.dTable = id;
                    status.Add(4); status.Add(5);
                    Req = _Admin.GetPartialTableData(status, page, pageSize, search, reg, reqtype);
                    break;
                case 4:  //Conclude
                    ViewBag.TPage = Math.Ceiling(Tcount[3] / 5.0);
                    ViewBag.dTable = id;
                    status.Add(6);
                    Req = _Admin.GetPartialTableData(status, page, pageSize, search, reg, reqtype);
                    break;
                case 5:   //To-close
                    ViewBag.TPage = Math.Ceiling(Tcount[4] / 5.0);
                    ViewBag.dTable = id;
                    status.Add(3); status.Add(7); status.Add(8);
                    Req = _Admin.GetPartialTableData(status, page, pageSize, search, reg, reqtype);
                    break;
                case 6:   //Unpaid
                    ViewBag.TPage = Math.Ceiling(Tcount[5] / 5.0);
                    ViewBag.dTable = id;
                    status.Add(9);
                    Req = _Admin.GetPartialTableData(status, page, pageSize, search, reg, reqtype);
                    break;
            }

            ViewBag.CurrentPage = page;
            ViewBag.TPage = Math.Ceiling(Req.filterCount / 5.0);
            var region = _Admin.getAllRegion();
            var dashData = new DashTable { Tdata = Req.Tdata.ToList(), Regions = region };

            return PartialView("TablePartial", dashData);
        }
        #endregion


        public IActionResult requestSupport()
        {
            return PartialView("PopupReqSupport");
        }

        #region SendLink
        public IActionResult SendLink()
        {
            return PartialView("PopupSendlink");
        }

        public IActionResult SendLinkData(string email, string name)
        {
            int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            Nullable<int> Phyid = null;
            Nullable<int> reqid = null;
            int roleId = Convert.ToInt32(Request.Cookies["CookieRole"]);
            string sub = "Submit Requset";
            string link = Url.Action("SubmitReq", "Patient", null, Request.Scheme);
            var body = $"Hi {name}, Please click on the following link to submit the request." + link;
            string filepath = null;
            _Genral.SendEmailOffice365(email, sub, body, null);
            _Genral.addEmailLog(body, sub, email, filepath, roleId, reqid, AdminId, Phyid);
            return Ok();
        }
        #endregion

        #region NewRequest
        public IActionResult NewRequest(int reqid)
        {
            return PartialView("NewRequest");
        }

        [HttpPost]
        public IActionResult NewReqData(ClientInformation client)
        {
            if (ModelState.IsValid)
            {
                if (!_Genral.CheckAvalibleRegion(client.State))
                {
                    ModelState.Clear();
                    return Json(new { value = "Region" });
                }
                else
                {
                    ModelState.Clear();
                    Request request;
                    if (_Genral.CheckExistAspUser(client.Email))
                    {
                        int userId = _Patient.FindUserId(client.Email);
                        request = _Patient.AddRequest(client, userId);
                        _Patient.AddRequestClient(client, request.Requestid);
                        return Json(new { value = "EmailExist" });
                    }
                    else
                    {
                        Aspnetuser AspUser = _Patient.AddAspUser(client);
                        User user = _Patient.AddUser(client, AspUser.Id);
                        request = _Patient.AddRequest(client, user.Userid);
                        _Patient.AddRequestClient(client, request.Requestid);
                        _Patient.addAspnetUserrole(user.Userid, 3);
                        return Json(new { value = "Ok" });
                    }
                }
            }
            else
            {
                return PartialView("NewRequest", client);
            }
        }
        #endregion


        #region export Excel
        public IActionResult Export(int id, string search, string reg, int reqtype)
        {
            List<short> status = new List<short>();
            switch (id)
            {
                case 1:   //New
                    status.Add(1);
                    break;
                case 2:   //Pending
                    status.Add(2);
                    break;
                case 3:   //Active
                    status.Add(4); status.Add(5);
                    break;
                case 4:  //Conclude
                    status.Add(6);
                    break;
                case 5:   //To-close
                    status.Add(3); status.Add(7); status.Add(8);
                    break;
                case 6:   //Unpaid
                    status.Add(9);
                    break;
            }
            DashTable data = _Admin.ExportPartialTableData(status, 0, 0, search, reg, reqtype);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TableData");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Requested Date";
                worksheet.Cell(1, 5).Value = "Physician Name";
                worksheet.Cell(1, 6).Value = "Date Of Service";
                worksheet.Cell(1, 7).Value = "Region";
                worksheet.Cell(1, 8).Value = "Phone No";
                worksheet.Cell(1, 9).Value = "Address";
                worksheet.Cell(1, 10).Value = "Notes";

                int row = 2;
                foreach (var item in data.Tdata)
                {
                    worksheet.Cell(row, 1).Value = item.Name;
                    worksheet.Cell(row, 2).Value = item.Intdate + "/" + item.Strmonth + "/" + item.Intyear;
                    worksheet.Cell(row, 3).Value = item.Requestor;
                    worksheet.Cell(row, 4).Value = item.RequestedDate;
                    worksheet.Cell(row, 5).Value = item.PhysicianName;
                    worksheet.Cell(row, 6).Value = item.DateOfService;
                    worksheet.Cell(row, 7).Value = item.Region;
                    worksheet.Cell(row, 8).Value = item.Phonenumber;
                    worksheet.Cell(row, 9).Value = item.Address;
                    worksheet.Cell(row, 10).Value = item.Notes;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var mimeType = "application/....";
                    return File(content, mimeType, "Reporrt.xlsx");
                }
            }
        }

        public IActionResult ExportAll(int id)
        {
            List<short> status = new List<short>();
            switch (id)
            {
                case 1:   //New
                    status.Add(1);
                    break;
                case 2:   //Pending
                    status.Add(2);
                    break;
                case 3:   //Active
                    status.Add(4); status.Add(5);
                    break;
                case 4:  //Conclude
                    status.Add(6);
                    break;
                case 5:   //To-close
                    status.Add(3); status.Add(7); status.Add(8);
                    break;
                case 6:   //Unpaid
                    status.Add(9);
                    break;
            }
            DashTable data = _Admin.ExportPartialTableData(status, 0, 0, null, null, 0);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TableData");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Requested Date";
                worksheet.Cell(1, 5).Value = "Physician Name";
                worksheet.Cell(1, 6).Value = "Date Of Service";
                worksheet.Cell(1, 7).Value = "Region";
                worksheet.Cell(1, 8).Value = "Phone No";
                worksheet.Cell(1, 9).Value = "Address";
                worksheet.Cell(1, 10).Value = "Notes";

                int row = 2;
                foreach (var item in data.Tdata)
                {
                    worksheet.Cell(row, 1).Value = item.Name;
                    worksheet.Cell(row, 2).Value = item.Intdate + "/" + item.Strmonth + "/" + item.Intyear;
                    worksheet.Cell(row, 3).Value = item.Requestor;
                    worksheet.Cell(row, 4).Value = item.RequestedDate;
                    worksheet.Cell(row, 5).Value = item.PhysicianName;
                    worksheet.Cell(row, 6).Value = item.DateOfService;
                    worksheet.Cell(row, 7).Value = item.Region;
                    worksheet.Cell(row, 8).Value = item.Phonenumber;
                    worksheet.Cell(row, 9).Value = item.Address;
                    worksheet.Cell(row, 10).Value = item.Notes;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var mimeType = "application/....";
                    return File(content, mimeType, "Reporrt.xlsx");
                }
            }
        }
        #endregion


        [ActionName("ViewCase")]
        public IActionResult ViewCase(int reqid)
        {
            ViewBag.reqid = reqid;
            var data = _Genral.getClientProfile(_Genral.getClientEmailbyReqId(reqid));
            return PartialView("_AViewCase", data);
        }


        #region viewNote
        [ActionName("ViewNotes")]
        public IActionResult ViewNotes(int reqid)
        {
            var data = _Admin.getAllNotes(reqid);
            return PartialView("_AViewNotes", data);
        }

        public IActionResult ViewNotedata(string note, int reqid)
        {
            if (note != null)
            {
                _Admin.addNote(reqid, note);
                return RedirectToAction("ViewNotes", new { reqid = reqid });
            }
            else
            {
                TempData["msg"] = "Please Enter Note!";
                return RedirectToAction("ViewNotes", new { reqid = reqid });
            }
        }
        #endregion


        #region CancelCase
        public IActionResult PopupCancelcase(int reqid)
        {
            var client = _Admin.GetClientById(reqid);
            var caseTag = _Admin.getAllCaseTag();
            var popupModel = new popupModel { Requestclient = client, Casetags = caseTag };
            return PartialView("PopupCancelcase", popupModel);
        }

        public IActionResult CancelReq(string CancelNote, string tag, int reqid)
        {
            if (tag != null)
            {
                int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
                Nullable<int> phyId = null;
                short Cancelstatus = 3;
                string CancelNotes = tag + CancelNote;
                _Genral.AddreqLogStatus(reqid, CancelNotes, AdminId, phyId, Cancelstatus);
                _Genral.updateReqStatus(reqid, Cancelstatus);
                return Json(new { value = "Ok" });
            }
            else
            {
                return Json(new { value = "Error" });
            }

        }
        #endregion


        #region Assign Physician

        public IActionResult PopupAssigncase(int reqid, string phyCase)
        {
            var client = _Admin.GetClientById(reqid);
            var region = _Admin.getAllRegion();
            var popupModel = new popupModel { Requestclient = client, Regions = region };
            if (phyCase == "AssignPhy")
            {
                ViewBag.PhyCase = "AssignPhy";
            }
            else if (phyCase == "TransferPhy")
            {
                ViewBag.PhyCase = "TransferPhy";
            }
            return PartialView("PopupAssigncase", popupModel);
        }

        public IActionResult CheckPhysician(int region)
        {
            var phy = _Admin.GetAvaliablePhysician(region);
            return Json(new { physician = phy });
        }

        public IActionResult AssignReq(string AssignNote, string phycase, int TransphyId, int reqid)
        {
            if (TransphyId != 0)
            {
                int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
                Nullable<int> phyId = null;
                short Assignstatus = 2;
                _Genral.AddreqLogStatus(reqid, AssignNote, Assignstatus, AdminId, phyId, TransphyId);
                _Genral.updateReqStatusWithPhysician(reqid, TransphyId, Assignstatus);
                return Json(new { value = "Ok" });
            }
            else
            {
                return Json(new { value = "Error" });
            }
        }
        #endregion


        #region Block Case
        public IActionResult PopupBlockcase(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
            return PartialView("PopupBlockcase", data);
        }

        public IActionResult BlockReq(int reqid, string note)
        {
            if (note != null)
            {
                short Blockstatus = 11;
                _Admin.AddBlockRequest(reqid, note);
                _Genral.updateReqStatus(reqid, 11);
                return Json(new { value = "Ok" });
            }
            else
            {
                return Json(new { value = "Error" });
            }
        }
        #endregion


        #region ViewUpload
        public IActionResult ViewUploads(int reqid)
        {
            var reqFile = _Genral.GetRequestsFileswithReq(reqid);
            return PartialView("_AViewUploads", reqFile);
        }


        public IActionResult deleteDoc(string file, int reqid)
        {
            var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file);
            if (System.IO.File.Exists(filePath))
            {
                _Admin.DeleteDocFile(file, reqid);
            }
            else
            {
                Console.WriteLine("File path not found");
                return NotFound();
            }
            return RedirectToAction("ViewUploads", new { reqid = reqid });
        }


        public IActionResult deleteAllDoc(List<string> file, int reqid)
        {
            for (var i = 0; i < file.Count(); i++)
            {
                var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file[i]);
                if (System.IO.File.Exists(filePath))
                {
                    _Admin.DeleteDocFile(file[i], reqid);
                }
                else
                {
                    Console.WriteLine("File path not found");
                    return NotFound();
                }
            }
            return RedirectToAction("ViewUploads", new { reqid = reqid });
        }

        public IActionResult SendMailDoc(List<string> file, int reqid)
        {
            int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            Nullable<int> Phyid = null;
            int roleId = Convert.ToInt32(Request.Cookies["CookieRole"]);
            string recEmail = _Genral.getClientEmailbyReqId(reqid);
            string subject = "Documnet files";
            string body = "Check attached document...";

            if (file.Count() > 0)
            {
                _Genral.SendEmailOffice365(recEmail, subject, body, file);
                string fileName = null;
                foreach (var files in file)
                {
                    var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", files);
                    if (System.IO.File.Exists(filePath))
                    {
                        fileName += filePath + " ";
                    }
                }
                _Genral.addEmailLog(body, subject, recEmail, fileName, roleId, reqid, AdminId, Phyid);
                return Json(new { value = "Ok" });
            }
            else
            {
                return Json(new { value = "Error" });
            }


        }

        #endregion


        #region SendOrder
        public IActionResult SendOrder(int reqid)
        {
            var profession = _Admin.getAllHealthProfession();
            var Order = new OrderViewModel { Healthprofessionaltype = profession, requestId = reqid };
            return PartialView("_ASendOrder", Order);
        }

        public IActionResult checkHealthProfessional(int profession)
        {
            var profbus = _Admin.getHealthProfessionBussiness(profession);
            return Json(new { Healthprofessional = profbus });
        }

        public IActionResult UpdateSendOrder(int vendorid, int reqid)
        {
            var vendor = _Admin.getVendorDetail(vendorid);
            return Json(new { Healthprofessional = vendor });
        }

        public IActionResult AddOrder(int vendorid, int reqid, string prescription, int refil)
        {
            if (vendorid == 0)
            {
                return Json(new { value = "ErrorV" });
            }
            else if (prescription == null)
            {
                return Json(new { value = "ErrorP" });
            }
            else
            {
                int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
                _Admin.AddOrderDetail(vendorid, AdminId, reqid, prescription, refil);
                return RedirectToAction("SendOrder", new { reqid = reqid });
            }
        }
        #endregion


        #region Clear Case
        public IActionResult clearcase(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
            return PartialView("PopupClearcase", data);
        }

        public IActionResult clearRequest(int reqid)
        {
            short ClearStatus = 9;
            int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            Nullable<int> phyId = null;
            _Genral.AddreqLogStatus(reqid, null, AdminId, phyId, ClearStatus);
            _Genral.updateReqStatus(reqid, ClearStatus);
            return Json(new { value = "Ok" });
        }
        #endregion


        #region SendAgreement
        public IActionResult sendAgree(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
            return PartialView("PopupSendAgreement", data);
        }

        public IActionResult AgrrementSent(int reqid, string email, string phone)
        {
            string subject = "Agreement Acknowledgement";
            string link = Url.Action("ReviewAgreement", "Home", new { reqid = reqid }, Request.Scheme);
            string body = $"Hi,<br /><br />Please click on the following link For Agreement :<br /><br />" + link;
            _Genral.SendEmailOffice365(email, subject, body, null);
            return Json(new { value = "done" });
        }
        #endregion


        #region Close case
        public IActionResult CloseCase(int reqid)
        {
            var UserEmail = _Genral.getClientEmailbyReqId(reqid);
            ClientInformation client = _Genral.getClientProfile(UserEmail);
            List<Request> reqFile = _Genral.GetRequestsFileswithReq(reqid);
            var data = new ClosecaseViewModel { ClientInformation = client, requests = reqFile };
            return PartialView("_ACloseCase", data);
        }
        public IActionResult ClosecaseUpdate(int reqid, string email, string phone)
        {
            string clientEmail = _Genral.getClientEmailbyReqId(reqid);
            _Genral.UpdateRequestClient(clientEmail, email, phone);
            return Json(new { value = "done" });
        }

        public IActionResult ClosecaseDone(int reqid)
        {
            short UnpaidStatus = 9;
            Nullable<int> phyId = null;
            int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            _Genral.updateReqStatus(reqid, UnpaidStatus);
            _Genral.AddreqLogStatus(reqid, null, AdminId, phyId, UnpaidStatus);
            return Json(new { value = "done" });
        }
        #endregion


        #region Encounter
        public IActionResult Encounter(int reqid)
        {
            if (!_Genral.CheckEncounterFinalize(reqid))
            {
                TempData["reqid"] = reqid;
                var client = _Genral.getClientProfile(_Genral.getClientEmailbyReqId(reqid));
                var encounter = _Genral.getEncounterDetail(reqid);
                var modelData = new EncounterViewModel { ClientInformation = client, EncounterForm = encounter };
                return PartialView("_AEncounter", modelData);
            }
            else
            {
                TempData["reqid"] = reqid;
                return PartialView("PopupEncounterFinalize");
            }
        }

        public IActionResult EncounterDone(EncounterViewModel data)
        {
            int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            Nullable<int> phyId = null;
            if (_Genral.CheckEncounterForm((int)data.EncounterForm.RequestId))
            {
                _Genral.UpdateEncounterForm(data.EncounterForm, AdminId, phyId);
            }
            else
            {
                _Genral.AddEncounterForm(data.EncounterForm, AdminId, phyId);
            }

            return Ok();
        }

        public IActionResult EncounFinalze(int reqid)
        {
            _Genral.EncounterFinalize(reqid);
            return Ok();
        }

        public IActionResult EncounterDownload(int reqid)
        {
            // Convert HTML to PDF using Rotativa
            //var pdfBytes = new Rotativa.ViewAsPdf("_Encounter", new { reqid = reqid })
            //{
            //    FileName = "MedicalReport.pdf"
            //}.BuildPdf(ControllerContext);

            //// Return the PDF as a FileResult
            //return File(pdfBytes, "application/pdf");
            //return new ActionAsPdf("Encounter", new { reqid = reqid }) { FileName = "MediaclReport.pdf" };
            return Ok();
        }
        #endregion
    }
}
