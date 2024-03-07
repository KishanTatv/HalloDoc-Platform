
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
            if (ViewBag.dTable != null)
            {
                DashbordData(2, 0);
                return PartialView("TablePartial", dashData);
            }
            return View(dashData);
        }


        public IActionResult DashbordData(int id, int page)
        {
            var Tcount = _Admin.TotalCountPatient();
            int pageSize = 5;

            IEnumerable<tableData> Req = new List<tableData>();

            switch (id)
            {
                case 1:   //New
                    ViewBag.TPage = Math.Ceiling(Tcount[0] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableData(page, pageSize);
                    break;
                case 2:   //Pending
                    ViewBag.TPage = Math.Ceiling(Tcount[1] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataPending(page, pageSize);
                    break;
                case 3:   //Active
                    ViewBag.TPage = Math.Ceiling(Tcount[2] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataActive(page, pageSize);
                    break;
                case 4:  //Conclude
                    ViewBag.TPage = Math.Ceiling(Tcount[3] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataConclude(page, pageSize);
                    break;
                case 5:   //To-close
                    ViewBag.TPage = Math.Ceiling(Tcount[4] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataToclose(page, pageSize);
                    break;
                case 6:   //Unpaid
                    ViewBag.TPage = Math.Ceiling(Tcount[5] / 5.0);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataUnpaid(page, pageSize);
                    break;
            }

            var region = _Admin.getAllRegion();
            var dashData = new DashTable { Tdata = Req.ToList(), Regions = region };

            return PartialView("TablePartial", dashData);
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
                return Json(new { value = "Ok" });
            }
            else
            {
                TempData["msg"] = "Please Enter Note!";
                var data = _Admin.getAllNotes(reqid);
                return PartialView("_AViewNotes", data);
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
                short Cancelstatus = 3;
                string CancelNotes = tag + CancelNote;
                _Admin.AddreqLogStatus(reqid, CancelNotes, AdminId, Cancelstatus);
                _Admin.updateReqStatus(reqid, Cancelstatus);
                return Json(new { value = "Ok" });
            }
            else
            {
                TempData["msg"] = "Please Select Reason !";
                var client = _Admin.GetClientById(reqid);
                var caseTag = _Admin.getAllCaseTag();
                var popupModel = new popupModel { Requestclient = client, Casetags = caseTag };
                return PartialView("PopupCancelcase", popupModel);
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

        public IActionResult AssignReq(string AssignNote, string phycase, int phyId, int reqid)
        {
            if (phyId != 0)
            {
                int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
                short Assignstatus = 2;
                if (phycase == "AssignPhy")
                {
                    _Admin.AddreqLogStatus(reqid, AssignNote, Assignstatus, AdminId, phyId);
                }
                else if(phycase == "TransferPhy")
                {

                }
                _Admin.updateReqStatusWithPhysician(reqid, phyId, Assignstatus);
                return Json(new { value = "Ok" });
            }
            else
            {
                TempData["msg"] = "Please Select Physician !";
                var client = _Admin.GetClientById(reqid);
                var region = _Admin.getAllRegion();
                var popupModel = new popupModel { Requestclient = client, Regions = region };
                return PartialView("PopupAssigncase", popupModel);
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
                _Admin.updateReqStatus(reqid, 11);
                return Json(new { value = "Ok" });
            }
            else
            {
                TempData["msg"] = "Please Enter Note !";
                var data = _Admin.GetClientById(reqid);
                return PartialView("PopupBlockcase", data);
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
            string recEmail = _Genral.getClientEmailbyReqId(reqid);
            string subject = "Documnet files";
            string body = "Check attached document...";
            _Genral.SendEmailOffice365(recEmail, subject, body, file);
            return RedirectToAction("ViewUploads", new { reqid = reqid });
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
            if (prescription != null)
            {
                int AdminId = _Admin.getAdminId(Request.Cookies["CookieEmail"]);
                _Admin.AddOrderDetail(vendorid, AdminId, reqid, prescription, refil);
                return Json(new { value = "Ok" });
            }
            else
            {
                TempData["msg"] = "Please Add Prescription !";
                return View();
            }
        }
        #endregion


        //public IActionResult PopupTransfercase(int reqid)
        //{
        //    var client = _Admin.GetClientById(reqid);
        //    var region = _Admin.getAllRegion();
        //    var popupModel = new popupModel { Requestclient = client, Regions = region };
        //    return PartialView("PopupTransfercase", popupModel);
        //}


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
