using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminDashController : Controller
    {
        private readonly ILogger<AdminDashController> _logger;
        private readonly IPatient patient;
        public AdminDashController(ILogger<AdminDashController> logger, IPatient patient)
        {
            _logger = logger;
            this.patient = patient;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashbord()
        {
            return View();
        }
    }
}
