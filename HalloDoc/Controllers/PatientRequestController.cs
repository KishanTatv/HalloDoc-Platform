using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Models;
using HalloDoc.Repository.Implement;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> PatientReq(ClientInformation Clientinfo, List<IFormFile> DocFile)
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

            if (DocFile != null)
            {
                foreach (var File in DocFile)
                {
                    if (File != null && File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", File.FileName);
                        //Using Buffering
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await File.CopyToAsync(stream);
                            patient.AddDocFile(File, request.Requestid);
                        }
                    }
                }
            }
            return RedirectToAction("Dashbord", "PatientDash");
        }

        [HttpGet]
        public IActionResult FamilyReq()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> FamilyReq(FormFCB FInfo, List<IFormFile> DocFile)
        {
            Request request;
            if (patient.CheckExistAspUser(FInfo.clientInformation.Email))
            {
                request = patient.AddOnlyFcbRequest(FInfo, 3);
            }
            else
            {
                User user = patient.AddFcbUser(FInfo);
                request = patient.AddFcbRequest(FInfo, user.Userid, 3);
                patient.AddFcbRequestClient(FInfo, request.Requestid);
            }

            if (DocFile != null)
            {
                foreach (var File in DocFile)
                {
                    if (File != null && File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", File.FileName);
                        //Using Buffering
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await File.CopyToAsync(stream);
                            patient.AddDocFile(File, request.Requestid);
                        }
                    }
                }
            }
            return RedirectToAction("PatientLogin");
        }


        [HttpGet]
        public IActionResult ConciergeReq()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConciergeReq(FormFCB FInfo, List<IFormFile> DocFile)
        {
            Request request;
            if (patient.CheckExistAspUser(FInfo.clientInformation.Email))
            {
                request = patient.AddOnlyFcbRequest(FInfo, 4);
                Concierge con = patient.AddConcierge(FInfo);
                patient.AddRequestConcierge(FInfo, request.Requestid, con.Conciergeid);
            }
            else
            {
                User user = patient.AddFcbUser(FInfo);
                request = patient.AddFcbRequest(FInfo, user.Userid, 4);
                patient.AddFcbRequestClient(FInfo, request.Requestid);
                Concierge con = patient.AddConcierge(FInfo);
                patient.AddRequestConcierge(FInfo, request.Requestid, con.Conciergeid);
            }

            if (DocFile != null)
            {
                foreach (var File in DocFile)
                {
                    if (File != null && File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", File.FileName);
                        //Using Buffering
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await File.CopyToAsync(stream);
                            patient.AddDocFile(File, request.Requestid);
                        }
                    }
                }
            }
            return RedirectToAction("PatientLogin");
        }



        [HttpGet]
        public IActionResult BussinessReq()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BussinessReq(FormFCB FInfo, List<IFormFile> DocFile)
        {
            Request request;
            if (patient.CheckExistAspUser(FInfo.clientInformation.Email))
            {
                request = patient.AddOnlyFcbRequest(FInfo, 1);
                Business bus = patient.AddBussiness(FInfo);
                patient.AddRequestBussiness(FInfo, request.Requestid, bus.Businessid);
            }
            else
            {
                User user = patient.AddFcbUser(FInfo);
                request = patient.AddFcbRequest(FInfo, user.Userid, 1);
                patient.AddFcbRequestClient(FInfo, request.Requestid);
                Business bus = patient.AddBussiness(FInfo);
                patient.AddRequestBussiness(FInfo, request.Requestid, bus.Businessid);
            }

            if (DocFile != null)
            {
                foreach (var File in DocFile)
                {
                    if (File != null && File.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", File.FileName);
                        //Using Buffering
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await File.CopyToAsync(stream);
                            patient.AddDocFile(File, request.Requestid);
                        }
                    }
                }
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
