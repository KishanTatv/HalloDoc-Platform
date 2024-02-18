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
        private readonly IPatient patient;

        public PatientDashController(ILogger<PatientDashController> logger, IPatient patient)
        {
            _logger = logger;
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
        public async Task<IActionResult> PatientDashbord()
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
        public IActionResult CheckPatientReq(string btn)
        {
            ViewBag.DefaultFieldValue = "reqMap";
            if (btn == "Me")
            {
                ViewBag.ShowAdditionalInput = false;
            }
            else if (btn == "Someoneelse")
            {
                ViewBag.ShowAdditionalInput = true;
            }
            return View("PatientReq", "PatientRequest");
        }



        [HttpPost]
        [ActionName("UpadteProfile")]
        public IActionResult UpadteProfile([Bind("User")] PatientDash userInfo)
        {
            var UserEmail = HttpContext.Session.GetString("SessionKeyEmail");

            Aspnetuser asp = patient.UpdateAspUser(userInfo, UserEmail);
            patient.UpdateUser(userInfo, UserEmail, asp.Id);

            return RedirectToAction("PatientDashbord");
        }



        [Route("ViewDocument/{id ?}")]
        [ActionName("ViewDocument")]
        public IActionResult ViewDocument(int id)
        {
            var UserEmail = HttpContext.Session.GetString("SessionKeyEmail");
            var ReqFile = patient.GetRequestsFileswithReq(UserEmail, id);
            var UserData = patient.GetUserByEmail(UserEmail);

            var PatientDash = new PatientDash { ReqWithFiles = ReqFile, User = UserData };
            return View(PatientDash);
        }


        [Route("UploadDoc/{id ?}")]
        [ActionName("UploadDoc")]
        public async Task<IActionResult> UploadDoc(List<IFormFile> DocFile, int id)
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
                        patient.AddDocFile(File, id);
                    }
                }
            }
            return RedirectToAction("ViewDocument", "Patient", new { id = id });
        }


        [Route("DownloadDocument/{file ?}")]
        [ActionName("DownloadDocument")]
        public IActionResult DownloadDocument(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", file);
            if (System.IO.File.Exists(path))
            {
                var mimeType = "application/....";
                return File(new FileStream(path, FileMode.Open), mimeType, file);
            }
            else
            {
                Console.WriteLine($"File not found: {path}");
                return NotFound();
            }
        }


        [HttpPost]
        [ActionName("DownloadAllFile")]
        public IActionResult DownloadAllFile()
        {
            //var fileList = AllFileByReqid(id);
            var fileList = Request.Form["selectCheckFile"].ToList();
            var zipName = $"MyFiles_{DateTime.Now:yyyyMMdd-HHmmss}.zip";

            using (var ms = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in fileList)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", file);
                        var entry = zipArchive.CreateEntry(file);
                        using (var entryStream = entry.Open())
                        using (var fileStream = new FileStream(path, FileMode.Open))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }

                return File(ms.ToArray(), "application/zip", zipName);
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
