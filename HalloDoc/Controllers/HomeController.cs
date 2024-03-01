using HalloDoc.Entity.Models;
using HalloDoc.Models;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Compression;

namespace HalloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenral _genral;

        public HomeController(ILogger<HomeController> logger, IGenral genral)
        {
            _logger = logger;
            _genral = genral;
        }

        public IActionResult Index()
        {
            return View();
        }


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
                        _genral.AddDocFile(File, id);
                    }
                }
            }

            int AdminId = (int)HttpContext.Session.GetInt32("SessionKeyAdminId");
            if (AdminId == null)
            {
                return RedirectToAction("ViewDocument", "PatientDash", new { id = id });
            }
            return RedirectToAction("A_ViewUploads", "AdminDash", new { reqid = id });
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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}