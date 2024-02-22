using ClosedXML.Excel;
using HalloDoc.Entity.AdminDashTable;
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

        public IActionResult Dashbord(int id,int page = 0)
        {
            int pageSize = 5;
            var Tcount = _Admin.TotalCountPatient();

            IEnumerable<tableData> Req = new List<tableData>();

            //new
            if (id == 1)
            {
                ViewBag.dTable = 1;
                Req = _Admin.GetTableData(1,page,pageSize);
            }

            //pending
            if (id == 2)
            {
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataPending(id);
            }

            //Active
            else if(id == 3)
            {
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataActive();
            }

            //conclude
            else if (id == 4)
            {
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataConclude();
            }

            //To-close
            else if (id == 5)
            {
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataToclose();
            }

            //Unpaid
            else if( id == 6)
            {
                ViewBag.dTable = id;
                Req = _Admin.GetTableDataUnpaid();
            }

            else {
                ViewBag.dTable = 1;
                Req = _Admin.GetTableData(1, page, pageSize);
            }

            var dashData = new DashTable { Tdata= Req.ToList() ,ToatlCount = Tcount};
            return View(dashData);
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


        [Route("ViewCase/{id ?}")]
        [ActionName("ViewCase")]
        public IActionResult ViewCase(int id)
        {
            var data = _Admin.GetClientById(id);
            return View(data); 
        }

        [Route("ViewNotes/{id ?}")]
        [ActionName("ViewNotes")]
        public IActionResult ViewNotes(int id)
        {
            return View();
        }


        public IActionResult Provider()
        {
            return View();
        }
    }
}
