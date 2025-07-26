using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace Phoenix.Controllers
{
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Download the latest log file
        [HttpGet]
        public IActionResult DownloadLatest()
        {
            var logDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDir))
                return NotFound("Log directory not found.");

            var latestLog = Directory.GetFiles(logDir, "log-*.txt")
                                     .OrderByDescending(f => f)
                                     .FirstOrDefault();

            if (latestLog == null)
                return NotFound("No log file found.");

            var fileName = Path.GetFileName(latestLog);
            var mimeType = "text/plain";
            var fileBytes = System.IO.File.ReadAllBytes(latestLog);
            return File(fileBytes, mimeType, fileName);
        }

        // Download all log files as a zip
        [HttpGet]
        public IActionResult DownloadAll()
        {
            var logDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDir))
                return NotFound("Log directory not found.");

            var logFiles = Directory.GetFiles(logDir, "log-*.txt");
            if (!logFiles.Any())
                return NotFound("No log files found.");

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var filePath in logFiles)
                    {
                        var entry = archive.CreateEntry(Path.GetFileName(filePath));
                        using (var entryStream = entry.Open())
                        using (var fileStream = System.IO.File.OpenRead(filePath))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
                return File(ms.ToArray(), "application/zip", "logs.zip");
            }
        }
    }
}
