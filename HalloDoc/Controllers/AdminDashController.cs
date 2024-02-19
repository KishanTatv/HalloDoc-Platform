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
            IEnumerable<tableData> data = _Admin.GetTableData();  
            return View(data);
        }
    }
}
