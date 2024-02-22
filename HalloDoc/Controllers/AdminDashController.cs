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

        public IActionResult Dashbord(int id)
        {
            IEnumerable<tableData> Req;
            if (id == 1 || id ==0)
            {
                ViewBag.dTable = 1;
                Req = _Admin.GetTableData(1);
            }
            else
            {
                ViewBag.dTable = id;
                Req = _Admin.GetTableWithPhyData(id);
            }

            int ToatlNew = _Admin.TotalCountPatient(1);
            int TotalPending = _Admin.TotalCountPatient(2);
            int TotalActive = _Admin.TotalCountPatient(3);
            int TotalConclude = _Admin.TotalCountPatient(4);
            int TotalToclose = _Admin.TotalCountPatient(5);
            int TotalUnpaid = _Admin.TotalCountPatient(6);
            List<int> countList = new List<int>();
            countList.Add(ToatlNew); countList.Add(TotalPending); countList.Add(TotalActive); countList.Add(TotalConclude); countList.Add(TotalToclose);  countList.Add(TotalUnpaid);


            var dashData = new DashTable { Tdata= Req ,ToatlCount = countList};
            return View(dashData);
        }

        public IActionResult ExportAll()
        {
            IEnumerable<tableData> data = _Admin.GetTableData(1);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("TableData");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date of Birth";

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.Name;
                    row++;
                }

                workbook.SaveAs("MyTableData.xlsx");
            }
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
