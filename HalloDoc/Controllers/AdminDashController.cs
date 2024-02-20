using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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


        public enum repestType
        {
            Bussiness = 1, Patient, Family, Conciegre
        }

        public enum reqTypebg
        {
            bgBussiness = 1, bgPatient, bgFamily, bgConcierge
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashbord()
        {
            IEnumerable<tableData> newReq = _Admin.GetTableData(1);  
            IEnumerable<tableData> pendingReq = _Admin.GetTableWithPhyData(2);


            int ToatlNew = _Admin.TotalCountPatient(1);
            int TotalPending = _Admin.TotalCountPatient(2);
            int TotalActive = _Admin.TotalCountPatient(3);
            int TotalConclude = _Admin.TotalCountPatient(4);
            int TotalToclose = _Admin.TotalCountPatient(5);
            int TotalUnpaid = _Admin.TotalCountPatient(6);
            List<int> countList = new List<int>();
            countList.Add(ToatlNew); countList.Add(TotalPending); countList.Add(TotalActive); countList.Add(TotalConclude); countList.Add(TotalToclose);  countList.Add(TotalUnpaid);   

            var dashData = new DashTable { Tdata = newReq, ToatlCount = countList};
            return View(dashData);
        }

        public IActionResult Provider()
        {
            return View();
        }
    }
}
