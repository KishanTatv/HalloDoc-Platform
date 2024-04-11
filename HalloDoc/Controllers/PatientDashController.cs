using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("Patient:common", "Null")]
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
        public IActionResult Dashbord()
        {
            string UserEmail = Request.Cookies["CookieEmail"];
            ClientInformation client = genral.getUserProfile(UserEmail);
            IEnumerable<RequestWithFile> ReqFile = patient.GetRequestsFiles(UserEmail);

            var PatientDash = new PatientDash { ReqWithFiles = ReqFile, clientInfo = client };
            return View(PatientDash);
        }



        [HttpPost]
        [ActionName("UpadteProfile")]
        public IActionResult UpadteProfile(PatientDash userInfo)
        {
            if (genral.CheckAvalibleRegion(userInfo.clientInfo.State))
            {
                string UserEmail = Request.Cookies["CookieEmail"];

                genral.UpdateRequestClient(userInfo.clientInfo, UserEmail);
                genral.UpdateUser(userInfo.clientInfo, UserEmail);

            }
            return RedirectToAction("Dashbord");
        }


        [Route("ViewDocument/{id ?}")]
        [ActionName("ViewDocument")]
        public async Task<IActionResult> ViewDocument(int id)
        {

            string UserEmail = Request.Cookies["CookieEmail"];
            if (UserEmail != null)
            {
                var ReqFile = genral.GetRequestsFileswithReq(id);
                Requestclient UserData = genral.GetClientById(id);
                return View(ReqFile);
            }
            else
            {
                return RedirectToAction("PatientLogin", "Patient");
            }

            //var PatientDash = new PatientDash { ReqWithFiles = ReqFile, reqclient = UserData };
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
