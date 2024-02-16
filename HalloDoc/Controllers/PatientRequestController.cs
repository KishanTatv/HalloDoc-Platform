using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Models;
using HalloDoc.Repository.Implement;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDoc.Controllers
{
    public class PatientRequestController : Controller
    {
        private readonly ILogger<PatientRequestController> _logger;
        private readonly IPatient patient;
        public PatientRequestController(ILogger<PatientRequestController> logger, IPatient patient)
        {
            _logger = logger;
            this.patient = patient;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("PatientRequest/PatientReq")]
        public IActionResult PatientReq()
        {
            ViewBag.ShowAdditionalInput = false;
            return View();
        }

        [HttpPost]
        [ActionName("PatientReq")]
        public async Task<IActionResult> PatientReq(ClientInformation Clientinfo, IFormFile DocFile)
        {
            Request request;
            if (patient.CheckExistAspUser(Clientinfo.Email))
            {
                request = patient.AddOnlyRequest(Clientinfo);
            }
            else
            {
                Aspnetuser AspUser = patient.AddAspUser(Clientinfo);
                User user = patient.AddUser(Clientinfo, AspUser.Id);
                request = patient.AddRequest(Clientinfo, user.Userid);
                patient.AddRequestClient(Clientinfo, request.Requestid);
            }

            if (DocFile != null && DocFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwrootuploads", DocFile.FileName);
                //Using Buffering
                using (var stream = System.IO.File.Create(filePath))
                {
                    await DocFile.CopyToAsync(stream);
                    patient.AddDocFile(DocFile, request.Requestid);
                }
                return RedirectToAction("PatientLogin");
            }
            return RedirectToAction("PatientLogin");
        }

        [HttpGet]
        public IActionResult FamilyReq()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConciergeReq()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BussinessReq()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FormDone(string button, FormFCB FInfo, IFormFile DocFile)
        {
            User user;
            if (patient.CheckExistAspUser(FInfo.clientInformation.Email))
            {
                if (button == "Familybtn")
                {
                    patient.AddFcbRequest(FInfo);
                }
            }
            else
            {
                user = patient.AddFcbUser(FInfo);

                if (button == "Familybtn")
                {
                    Request req = patient.AddFcbRequest(FInfo, user.Userid, 3);
                    patient.AddFcbRequestClient(FInfo, req.Requestid);
                }
                else if (button == "Conciergebtn")
                {
                    Request req = patient.AddFcbRequest(FInfo, user.Userid, 4);
                    patient.AddFcbRequestClient(FInfo, req.Requestid);
                    Concierge con = patient.AddConcierge(FInfo);
                    patient.AddRequestConcierge(FInfo, req.Requestid, con.Conciergeid);
                }
                else if (button == "Bussinessbtn")
                {
                    Request req = patient.AddFcbRequest(FInfo, user.Userid, 1);
                    patient.AddFcbRequestClient(FInfo, req.Requestid);
                    Business bus = patient.AddBussiness(FInfo);
                    patient.AddRequestBussiness(FInfo, req.Requestid, bus.Businessid);
                }


            }

            if (DocFile != null && DocFile.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwrootuploads", DocFile.FileName);
                //Using Buffering
                using (var stream = System.IO.File.Create(filePath))
                {
                    await DocFile.CopyToAsync(stream);
                    patient.AddDocFile(DocFile, request.Requestid);
                }
                return RedirectToAction("PatientLogin");
            }
            return RedirectToAction("PatientLogin");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
