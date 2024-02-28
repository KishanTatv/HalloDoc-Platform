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

            var dashData = new DashTable {ToatlCount = Tcount };
            return View(dashData);
        }


        public IActionResult DashbordData(int id,int page)
        {
            var Tcount = _Admin.TotalCountPatient();
            int pageSize = 5;

            IEnumerable<tableData> Req=new List<tableData>();

            //new
            if (id==1)
            {
                ViewBag.Tcount = Tcount[0];
                ViewBag.dTable = id;
                Req = _Admin.GetTableData(page, pageSize);
            }

            //pending
            else if (id == 2)
            {
                ViewBag.Tcount = Tcount[1];
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataPending(page, pageSize);
            }

            //Active
            else if (id == 3)
            {
                ViewBag.Tcount = Tcount[2];
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataActive(page, pageSize);
            }

            //conclude
            else if (id == 4)
            {
                ViewBag.Tcount = Tcount[3];
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataConclude(page, pageSize);
            }

            //To-close
            else if (id == 5)
            {
                ViewBag.Tcount = Tcount[4];
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataToclose(page, pageSize);
            }

            //Unpaid
            else if (id == 6)
            {
                ViewBag.Tcount = Tcount[5];
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataUnpaid(page, pageSize);
            }

            var caseTag = _Admin.getAllCaseTag();
            var region = _Admin.getAllRegion();
            var dashData = new DashTable { Tdata= Req.ToList(), Casetags = caseTag, Regions = region };

            return PartialView("TablePartial", dashData );
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


        [ActionName("ViewCase")]
        public IActionResult ViewCase(int reqid)
        {
            var data = _Admin.GetClientById(reqid);
            return View(data); 
        }


        [ActionName("ViewNotes")]
        public IActionResult ViewNotes(int reqid)
        {
            var data = _Admin.getAllNotes(reqid);
            return View(data);
        }


        public IActionResult ViewNotedata(string note, int reqid)
        {
            if(note != null)
            {
                _Admin.addNote(reqid, note);
            }
            var data = _Admin.getAllNotes(reqid);
            return PartialView("ViewNotes", data);
        }

        public IActionResult CancelReq(string CancelNotes, string tag, int reqid)
        {
            short Cancelstatus = 6;
            if(tag != null)
            {
                _Admin.CancelRequest(reqid, CancelNotes, tag, Cancelstatus);
            }
            return RedirectToAction("Dashbord");
        }

        public IActionResult CheckPhysician(int region)
        {
            var phy = _Admin.GetAvaliablePhysician(region);
            return Json(new { physician=phy });
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
