using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Windows.Networking;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Admin")]
    public class AdminDashController : Controller
    {
        private readonly ILogger<AdminDashController> _logger;
        private readonly IGenral _Genral;
        private readonly IAdmin _Admin;
        public AdminDashController(ILogger<AdminDashController> logger, IGenral _Genral, IAdmin _Admin)
        {
            _logger = logger;
            this._Genral = _Genral;
            this._Admin = _Admin;
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

            IEnumerable<tableData> Req = new List<tableData>();

            switch (id)
            {
                case 1:   //New
                    ViewBag.TPage = Math.Ceiling(Tcount[0] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableData();
                    if (search != null)
                    {
                        Req = Req.Where(e => e.Name.ToLower().Contains(search.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if(reg != null)
                    {
                        Req = Req.Where(e => e.Region.ToLower().Equals(reg.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if(reqtype != 0)
                    {
                        Req = Req.Where(e => e.ReqTypeId.Equals(reqtype));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    Req = Req.Skip(page * pageSize).Take(pageSize).ToList();
                    break;
                case 2:   //Pending
                    ViewBag.TPage = Math.Ceiling(Tcount[1] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataPending();
                    if (search != null)
                    {
                        Req = Req.Where(e => e.Name.ToLower().Contains(search.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reg != null)
                    {
                        Req = Req.Where(e => e.Region.ToLower().Equals(reg.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reqtype != 0)
                    {
                        Req = Req.Where(e => e.ReqTypeId.Equals(reqtype));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    Req = Req.Skip(page * pageSize).Take(pageSize).ToList();
                    break;
                case 3:   //Active
                    ViewBag.TPage = Math.Ceiling(Tcount[2] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataActive();
                    if (search != null)
                    {
                        Req = Req.Where(e => e.Name.ToLower().Contains(search.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reg != null)
                    {
                        Req = Req.Where(e => e.Region.ToLower().Equals(reg.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reqtype != 0)
                    {
                        Req = Req.Where(e => e.ReqTypeId.Equals(reqtype));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    Req = Req.Skip(page * pageSize).Take(pageSize).ToList();
                    break;
                case 4:  //Conclude
                    ViewBag.TPage = Math.Ceiling(Tcount[3] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataConclude();
                    if (search != null)
                    {
                        Req = Req.Where(e => e.Name.ToLower().Contains(search.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reg != null)
                    {
                        Req = Req.Where(e => e.Region.ToLower().Equals(reg.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reqtype != 0)
                    {
                        Req = Req.Where(e => e.ReqTypeId.Equals(reqtype));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    Req = Req.Skip(page * pageSize).Take(pageSize).ToList();
                    break;
                case 5:   //To-close
                    ViewBag.TPage = Math.Ceiling(Tcount[4] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataToclose();
                    if (search != null)
                    {
                        Req = Req.Where(e => e.Name.ToLower().Contains(search.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reg != null)
                    {
                        Req = Req.Where(e => e.Region.ToLower().Equals(reg.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reqtype != 0)
                    {
                        Req = Req.Where(e => e.ReqTypeId.Equals(reqtype));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    Req = Req.Skip(page * pageSize).Take(pageSize).ToList();
                    break;
                case 6:   //Unpaid
                    ViewBag.TPage = Math.Ceiling(Tcount[5] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataUnpaid();
                    if (search != null)
                    {
                        Req = Req.Where(e => e.Name.ToLower().Contains(search.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reg != null)
                    {
                        Req = Req.Where(e => e.Region.ToLower().Equals(reg.ToLower()));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    if (reqtype != 0)
                    {
                        Req = Req.Where(e => e.ReqTypeId.Equals(reqtype));
                        ViewBag.TPage = Math.Ceiling(Req.Count() / 5.0);
                    }
                    Req = Req.Skip(page * pageSize).Take(pageSize).ToList();
                    break;
            }

            var region = _Admin.getAllRegion();
            var dashData = new DashTable { Tdata = Req.ToList(), Regions = region };

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

        public IActionResult SendLinkData(string email)
        {
            int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
            Nullable<int> Phyid = null;
            Nullable<int> reqid = null;
            int roleId = Convert.ToInt32(Request.Cookies["CookieRole"]);
            string sub = "Submit Requset";
            string link = Url.Action("SubmitReq", "Patient", null, Request.Scheme);
            var body = $"Hi, Please click on the following link to submit the request." + link;
            string filepath = null;
            //_Genral.SendEmailOffice365(email, sub, body, null);
            _Genral.addEmailLog(body, sub, email, filepath, roleId, reqid, AdminId, Phyid);
            return Ok();
        }
        #endregion

        public IActionResult ExportAll()
        {
            //IEnumerable<tableData> data = _Admin.GetTableData(1);

            //using (var workbook = new XLWorkbook())
            //{
            //    var worksheet = workbook.Worksheets.Add("TableData");

            //    worksheet.Cell(1, 1).Value = "Name";
            //    worksheet.Cell(1, 2).Value = "Date of Birth";

            //    int row = 2;
            //    foreach (var item in data)
            //    {
            //        worksheet.Cell(row, 1).Value = item.Name;
            //        row++;
            //    }

            //    workbook.SaveAs("MyTableData.xlsx");
            //}
            return View();
        }


        [ActionName("ViewCase")]
        public IActionResult ViewCase(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
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
                if (phycase == "AssignPhy")
                {
                    short Assignstatus = 1;
                    _Genral.AddreqLogStatus(reqid, AssignNote, Assignstatus, AdminId, phyId, TransphyId);
                    _Genral.updateReqStatusWithPhysician(reqid, TransphyId, Assignstatus);
                }
                else if (phycase == "TransferPhy")
                {
                    short Transferstatus = 2;
                    _Genral.AddreqLogStatus(reqid, AssignNote, Transferstatus, AdminId, phyId, TransphyId);
                    _Genral.updateReqStatusWithPhysician(reqid, TransphyId, Transferstatus);
                }
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
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file);
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file[i]);
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
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", files);
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
            return Ok();
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
        #endregion

        public IActionResult NewRequest(int reqid)
        {
            return View();
        }

        public IActionResult Provider()
        {
            return View();
        }
    }
}
