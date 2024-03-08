using DocumentFormat.OpenXml.EMMA;
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
        private readonly IGenral genral;

        public PatientRequestController(ILogger<PatientRequestController> logger, IPatient patient, IGenral genral)
        {
            _logger = logger;
            this.patient = patient;
            this.genral = genral;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region Patient request
        [HttpGet]
        public IActionResult PatientReq()
        {
            ViewBag.ShowAdditionalInput = "false";
            return View();
        }


        [HttpPost]
        public IActionResult CheckPatientReq(string btn)
        {
            ViewBag.DefaultFieldValue = "reqMap";
            if (btn == "Me")
            {
                ViewBag.ShowAdditionalInput = "false";
            }
            else if (btn == "Someoneelse")
            {
                ViewBag.ShowAdditionalInput = "true";
            }
            return View("PatientReq");
        }


        [HttpPost]
        [ActionName("PatientReq")]
        public async Task<IActionResult> PatientReq(ClientInformation Clientinfo, List<IFormFile> DocFile)
        {
            if (ModelState.IsValid)
            {
                if (!genral.CheckAvalibleRegion(Clientinfo.State))
                {
                    TempData["Msg"] = "Not Avliable Region";
                    return View();
                }
                else if (Clientinfo.Password != Clientinfo.ConfirmPassword)
                {
                    TempData["Msg"] = "Password Mismatch";
                    return View();
                }
                else
                {
                    Request request;
                    if (genral.CheckExistAspUser(Clientinfo.Email))
                    {
                        int userId = patient.FindUserId(Clientinfo.Email);
                        request = patient.AddRequest(Clientinfo, userId);
                    }
                    else
                    {
                        Aspnetuser AspUser = patient.AddAspUser(Clientinfo);
                        User user = patient.AddUser(Clientinfo, AspUser.Id);
                        request = patient.AddRequest(Clientinfo, user.Userid);
                        patient.AddRequestClient(Clientinfo, request.Requestid);
                        patient.AddAspnetUserRole(AspUser.Id);
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
                                    genral.AddDocFile(File, request.Requestid);
                                }
                            }
                        }
                    }
                    return RedirectToAction("Dashbord", "PatientDash");
                }
                
            }
            else
            {
                return View();
            }
        }
        #endregion


        #region Family request
        [HttpGet]
        public IActionResult FamilyReq()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> FamilyReq(FormFCB FInfo, List<IFormFile> DocFile)
        {
            if (ModelState.IsValid)
            {
                if (genral.CheckAvalibleRegion(FInfo.clientInformation.State))
                {
                    if (genral.checkBlockReq(FInfo.PatientEmail))
                    {
                        TempData["Msg"] = "Blocked Request !";
                        return View();
                    }
                    else
                    {
                        Request request;
                        if (genral.CheckExistAspUser(FInfo.clientInformation.Email))
                        {
                            int userId = patient.FindUserId(FInfo.clientInformation.Email);
                            request = patient.AddFcbRequest(FInfo, userId, 3);
                        }
                        else
                        {
                            var userEmail = FInfo.clientInformation.Email;
                            var subject = "Create your Account";
                            string link = Url.Action("CreatePatient", "Patient");
                            var body = $"Hi,<br /><br />Please click on the following link to create your account:<br /><br />" + link;
                            patient.sendMail(userEmail, subject, body);

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
                                        genral.AddDocFile(File, request.Requestid);
                                    }
                                }
                            }
                        }
                        return RedirectToAction("PatientLogin", "Patient");
                    }
                }
                else
                {
                    TempData["Msg"] = "Not Avliable Region";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        #endregion


        #region Concierge request
        [HttpGet]
        public IActionResult ConciergeReq()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConciergeReq(FormFCB FInfo)
        {
            if (ModelState.IsValid)
            {
                if (genral.CheckAvalibleRegion(FInfo.clientInformation.State))
                {
                    if (genral.checkBlockReq(FInfo.PatientEmail))
                    {
                        TempData["Msg"] = "Blocked Request !";
                        return View();
                    }
                    else
                    {
                        Request request;
                        if (genral.CheckExistAspUser(FInfo.clientInformation.Email))
                        {
                            int userId = patient.FindUserId(FInfo.clientInformation.Email);
                            request = patient.AddFcbRequest(FInfo, userId, 4);
                            Concierge con = patient.AddConcierge(FInfo);
                            patient.AddRequestConcierge(FInfo, request.Requestid, con.Conciergeid);
                        }
                        else
                        {
                            var userEmail = FInfo.clientInformation.Email;
                            var subject = "Create your Account";
                            string link = Url.Action("CreatePatient", "Patient");
                            var body = $"Hi,<br /><br />Please click on the following link to create your account:<br /><br />" + link;
                            patient.sendMail(userEmail, subject, body);

                            User user = patient.AddFcbUser(FInfo);
                            request = patient.AddFcbRequest(FInfo, user.Userid, 4);
                            patient.AddFcbRequestClient(FInfo, request.Requestid);
                            Concierge con = patient.AddConcierge(FInfo);
                            patient.AddRequestConcierge(FInfo, request.Requestid, con.Conciergeid);
                        }
                        return RedirectToAction("PatientLogin", "Patient");
                    }
                }
                else
                {
                    TempData["Msg"] = "Not Avliable Region";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        #endregion


        #region Bussiness request
        [HttpGet]
        public IActionResult BussinessReq()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BussinessReq(FormFCB FInfo)
        {
            if (ModelState.IsValid)
            {
                if (genral.CheckAvalibleRegion(FInfo.clientInformation.State))
                {
                    if (genral.checkBlockReq(FInfo.PatientEmail))
                    {
                        TempData["Msg"] = "Blocked Request !";
                        return View();
                    }
                    else
                    {
                        Request request;
                        if (genral.CheckExistAspUser(FInfo.clientInformation.Email))
                        {
                            int userId = patient.FindUserId(FInfo.clientInformation.Email);
                            request = patient.AddFcbRequest(FInfo, userId, 1);
                            Business bus = patient.AddBussiness(FInfo);
                            patient.AddRequestBussiness(FInfo, request.Requestid, bus.Businessid);
                        }
                        else
                        {
                            var userEmail = FInfo.clientInformation.Email;
                            var subject = "Create your Account";
                            string link = Url.Action("CreatePatient", "Patient");
                            var body = $"Hi,<br /><br />Please click on the following link to create your account:<br /><br />" + link;
                            patient.sendMail(userEmail, subject, body);

                            User user = patient.AddFcbUser(FInfo);
                            request = patient.AddFcbRequest(FInfo, user.Userid, 1);
                            patient.AddFcbRequestClient(FInfo, request.Requestid);
                            Business bus = patient.AddBussiness(FInfo);
                            patient.AddRequestBussiness(FInfo, request.Requestid, bus.Businessid);
                        }
                        return RedirectToAction("PatientLogin", "Patient");
                    }
                }
                else
                {
                    TempData["Msg"] = "Not Avliable Region";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        #endregion




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
