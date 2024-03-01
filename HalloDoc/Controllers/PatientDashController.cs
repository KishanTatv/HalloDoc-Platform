using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace HalloDoc.Controllers
{
    public class PatientDashController : Controller
    {
        private readonly ILogger<PatientDashController> _logger;
        private readonly IGenral genral;
        private readonly IPatient patient;

        public PatientDashController(ILogger<PatientDashController> logger, IGenral genral, IPatient patient)
        {
            _logger = logger;
            this.genral = genral;
            this.patient = patient;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ViewDocument()
        {
            return View();
        }


        public IActionResult ReviewAgreement()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashbord()
        {
            var UserEmail = HttpContext.Session.GetString("SessionKeyEmail");
            if (UserEmail != null)
            {
                User UserData  = patient.GetUserByEmail(UserEmail);
                IEnumerable<RequestWithFile> ReqFile = patient.GetRequestsFiles(UserEmail);

                var PatientDash = new PatientDash { ReqWithFiles = ReqFile, User = UserData };
                return View(PatientDash);
            }
            return RedirectToAction("PatientLogin", "Patient");
        }



        [HttpPost]
        [ActionName("UpadteProfile")]
        public IActionResult UpadteProfile([Bind("User")] PatientDash userInfo)
        {
            var UserEmail = HttpContext.Session.GetString("SessionKeyEmail");

            patient.UpdateUser(userInfo, UserEmail);
            patient.UpdateRequestClient(userInfo, UserEmail);

            return RedirectToAction("Dashbord");
        }


        [Route("ViewDocument/{id ?}")]
        [ActionName("ViewDocument")]
        public async Task<IActionResult> ViewDocument(int id)
        {
            var UserEmail = HttpContext.Session.GetString("SessionKeyEmail");
            var ReqFile = genral.GetRequestsFileswithReq(id);
            Requestclient UserData = genral.GetClientById(id);

            //var PatientDash = new PatientDash { ReqWithFiles = ReqFile, reqclient = UserData };
            return View(ReqFile);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
