using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INDIACom.Models;
using System.IO;
using INDIACom.App_Cude;




namespace INDIACom.Controllers
{
    public class PaperVersionController : Controller
    {
        private readonly DAL paperVersionDAL = new DAL();
        private readonly DAL paperDAL = new DAL(); // Optional: for logging documents

        [HttpPost]
        public ActionResult SubmitNewVersion(HttpPostedFileBase PaperFile, int paperId, int eventId, int memberId)
        {
            string[] allowedPaperExtensions = { ".doc", ".docx" };

            if (PaperFile == null || PaperFile.ContentLength == 0)
            {
                ModelState.AddModelError("", "No file selected.");
                return View();
            }

            string extension = Path.GetExtension(PaperFile.FileName).ToLower();
            if (!allowedPaperExtensions.Contains(extension))
            {
                ModelState.AddModelError("PaperFile", "Only .doc and .docx files are allowed.");
                return View();
            }

            // Create directory if it doesn't exist
            string directoryPath = Server.MapPath("~/Application/Papers/");
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // Generate file name and save the file
            string documentType = "Paper";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string newFileName = $"{memberId}_{eventId}_{documentType}_{timestamp}{extension}";
            string fullPath = Path.Combine(directoryPath, newFileName);
            PaperFile.SaveAs(fullPath);

            string dbPath = "/Application/Papers/" + newFileName;

            // Insert new version (version 2, 3, etc.)
            string versionMessage = paperVersionDAL.SubmitPaperVersion(
                paperId: paperId,
                eventId: eventId,
                dateOfSubmission: DateTime.Now,
                path: dbPath,
                complianceReportPath: null
            );

            TempData["Message"] = versionMessage;

            // Redirect to the PaperDetails view or the relevant page after version submission
            return RedirectToAction("PaperDetails", "PaperSubmission", new { id = paperId });
        }
    }
}