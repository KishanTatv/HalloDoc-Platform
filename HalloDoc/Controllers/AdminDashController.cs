using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Models;
using HalloDoc.HelperClass;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace HalloDoc.Controllers
{
    public class AdminDashController : Controller
    {
        private readonly ILogger<AdminDashController> _logger;
        private readonly IAdmin _Admin;
        public AdminDashController(ILogger<AdminDashController> logger, IAdmin _Admin)
        {
            _logger = logger;
            this._Admin = _Admin;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Dashbord()
        {
            var Tcount = _Admin.TotalCountPatient();

            var dashData = new DashTable { ToatlCount = Tcount };
            if(ViewBag.dTable != null)
            {
                DashbordData(ViewBag.dTable, 0);
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
                    ViewBag.TPage = Math.Ceiling((double)Tcount[0] / 5);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableData(page, pageSize);
                    break;
                case 2:   //Pending
                    ViewBag.TPage = Math.Ceiling((double)Tcount[1] / 5);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataPending(page, pageSize);
                    break;
                case 3:   //Active
                    ViewBag.TPage = Math.Ceiling((double)Tcount[2] / 5);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataActive(page, pageSize);
                    break;
                case 4:  //Conclude
                    ViewBag.TPage = Math.Ceiling((double)Tcount[3] / 5);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataConclude(page, pageSize);
                    break;
                case 5:   //To-close
                    ViewBag.TPage = Math.Ceiling((double)Tcount[4] / 5);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataToclose(page, pageSize);
                    break;
                case 6:   //Unpaid
                    ViewBag.TPage = Math.Ceiling((double)Tcount[5] / 5);
                    ViewBag.dTable = id;
                    Req = _Admin.GetTableDataUnpaid(page, pageSize);
                    break;
            }

            var caseTag = _Admin.getAllCaseTag();
            var region = _Admin.getAllRegion();
            var dashData = new DashTable { Tdata = Req.ToList(), Casetags = caseTag, Regions = region };

            return PartialView("TablePartial", dashData);
        }

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


        [ActionName("A_ViewCase")]
        public IActionResult A_ViewCase(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
            return View(data);
        }


        [ActionName("A_ViewNotes")]
        public IActionResult A_ViewNotes(int reqid)
        {
            var data = _Admin.getAllNotes(reqid);
            return View(data);
        }

        public IActionResult ViewNotedata(string note, int reqid)
        {
            if (note != null)
            {
                _Admin.addNote(reqid, note);
            }
            var data = _Admin.getAllNotes(reqid);
            return PartialView("A_ViewNotes", data);
        }

        public IActionResult CancelReq(string CancelNote, string tag, int reqid)
        {
            int AdminId = (int)HttpContext.Session.GetInt32("SessionKeyAdminId");
            short Cancelstatus = 3;
            if (tag != null)
            {
                string CancelNotes = tag + CancelNote;
                _Admin.AddreqLogStatus(reqid, CancelNotes, AdminId, Cancelstatus);
                _Admin.updateReqStatus(reqid, Cancelstatus);
            }
            return RedirectToAction("Dashbord");
        }

        public IActionResult CheckPhysician(int region)
        {
            var phy = _Admin.GetAvaliablePhysician(region);
            return Json(new { physician = phy });
        }

        public IActionResult AssignReq(string AssignNote, int phyId, int reqid)
        {
            int AdminId = (int)HttpContext.Session.GetInt32("SessionKeyAdminId");
            short Assignstatus = 2;
            if (phyId != null)
            {
                _Admin.AddreqLogStatus(reqid, AssignNote, Assignstatus, AdminId, phyId);
                _Admin.updateReqStatusWithPhysician(reqid, phyId, Assignstatus);
            }
            return RedirectToAction("Dashbord");
        }

        public IActionResult popup_blockcase(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
            return PartialView("popup_blockcase", data);
        }

        public IActionResult BlockReq(int reqid, string note)
        {
            short Blockstatus = 11;
            _Admin.AddBlockRequest(reqid, note);
            _Admin.updateReqStatus(reqid, 11);
            return RedirectToAction("Dashbord");
        }

        public IActionResult A_ViewUploads(int reqid)
        {
            return View();
        }


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
