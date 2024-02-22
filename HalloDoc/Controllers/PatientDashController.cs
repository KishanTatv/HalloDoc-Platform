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
        public async Task<IActionResult> Dashbord()
        {
            var UserEmail = HttpContext.Session.GetString("SessionKeyEmail");
            if (UserEmail != null)
            {
                Requestclient UserData  = patient.GetUserByEmail(UserEmail);
                IEnumerable<RequestWithFile> ReqFile = patient.GetRequestsFiles(UserEmail);

                var PatientDash = new PatientDash { ReqWithFiles = ReqFile, Requestclient = UserData };
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
            var ReqFile = patient.GetRequestsFileswithReq(UserEmail, id);
            var UserData = patient.GetClientById(id);

            var PatientDash = new PatientDash { ReqWithFiles = ReqFile, Requestclient = UserData };
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
            return RedirectToAction("ViewDocument", "PatientDash", new { id = id });
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
