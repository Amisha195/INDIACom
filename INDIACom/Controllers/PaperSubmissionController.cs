using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INDIACom.Models;
using System.IO;
using INDIACom.App_Cude;
using System.Drawing.Printing;

public class PaperSubmissionController : Controller
{
    private readonly DAL paperDAL = new DAL();

    [HttpGet]
    
    public ActionResult SubmitPapers()
    {
        // Check if user is logged in
        if (Session["user"] == null)
        {
            // Not logged in — show custom view
            return View("NotLoggedIn");
        }

        var loggedInUser = (MemberModel)Session["user"];
        ViewBag.LoggedInMemberID = loggedInUser.MemberID;

        // Logged in — show paper submission form
        return View(); // Views/PaperSubmission/SubmitPapers.cshtml
    }
   
    [HttpGet]

    public JsonResult VerifyMemberID(string memberId)
    {
        if (string.IsNullOrWhiteSpace(memberId))
        {
            return Json(new { success = false, message = "Member ID is required." }, JsonRequestBehavior.AllowGet);
        }

        //DAL dal = new DAL();
        string message;
        string name = paperDAL.VerifyMemberByID(memberId, out message);

        if (!string.IsNullOrEmpty(name))
        {
            return Json(new { success = true, name = name, message = message }, JsonRequestBehavior.AllowGet);
        }
        else
        {
            return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }
    }







    //ends here 

    [AllowAnonymous]


    [HttpPost]



    public ActionResult SubmitPapers(FormCollection form, HttpPostedFileBase PaperFile, HttpPostedFileBase PlagiarismReport)
    {
        int TryParseInt(string input, string fieldName)
        {
            int parsedResult = 0;
            if (int.TryParse(input, out parsedResult))
            {
                return parsedResult;
            }
            else
            {
                ModelState.AddModelError(fieldName, $"Invalid value for {fieldName}.");
                return 0;
            }
        }
   
        int eventId, trackId, sessionId, memberId, correspondenceId;
        int.TryParse(form["Event_Id"], out eventId);
        int.TryParse(form["Track_Id"], out trackId);
        int.TryParse(form["Session_Id"], out sessionId);
        int.TryParse(form["Authors[0].MemberID"], out memberId);
        int.TryParse(form["CorrespondingAuthorID"], out correspondenceId);


        PaperSubmissionModel model = new PaperSubmissionModel
        {
            Title = form["Title"],
            DateOfSubmission = DateTime.Now,

            Event_Id = TryParseInt(form["Event_Id"], "Event_Id"),
            Track_Id = TryParseInt(form["Track_Id"], "Track_Id"),
            Session_Id = TryParseInt(form["Session_Id"], "Session_Id"),
            Event_Name = form["Event_Name"],
            Track_Name = form["Track_Name"],
            Session_Name = form["Session_Name"],
            Member_Id = TryParseInt(form["Authors[0].MemberID"], "Member_Id"),
            Correspondence_Id = TryParseInt(form["CorrespondingAuthorID"], "Correspondence_Id")
        };

        // === Validate Co-Authors (Before Paper is Saved) ===
        List<string> validCoAuthorIds = new List<string>();
        HashSet<string> seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < 8; i++) // You allow up to 8 co-authors
        {
            string rawId = form[$"Authors[{i}].MemberID"];

            if (string.IsNullOrWhiteSpace(rawId))
                continue;

            string trimmedId = rawId.Trim().ToUpperInvariant();

            // Skip if already seen (duplicate)
            if (!seenIds.Add(trimmedId))
            {
                ModelState.AddModelError($"Authors[{i}].MemberID", $"Duplicate co-author ID: {trimmedId}");
                continue;
            }

            // Check member validity in DB
            string message;
            string memberName = paperDAL.VerifyMemberByID(trimmedId, out message);

            if (string.IsNullOrEmpty(memberName))
            {
                ModelState.AddModelError($"Authors[{i}].MemberID", $"Invalid Member ID: {trimmedId}. {message}");
                continue;
            }

            // ✅ Valid member
            validCoAuthorIds.Add(trimmedId);
        }

        // Final check before continuing
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                message = "Submission failed due to invalid or duplicate co-author(s).",
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        // Save only valid co-authors
        model.Co_Authors_Id = string.Join(",", validCoAuthorIds);



        var allowedPaperExtensions = new[] { ".doc", ".docx" };
        var allowedPlagiarismExtensions = new[] { ".pdf" };

        // Ensure model values are set (fallback to Session if needed)
        if (model.Member_Id == 0 && Session["MemberId"] != null)
            model.Member_Id = Convert.ToInt32(Session["MemberId"]);

        if (model.Event_Id == 0 && Session["EventId"] != null)
            model.Event_Id = Convert.ToInt32(Session["EventId"]);

        // === Paper Upload ===
        if (PaperFile != null && PaperFile.ContentLength > 0)
        {
            string extension = Path.GetExtension(PaperFile.FileName).ToLower();
            if (!allowedPaperExtensions.Contains(extension))
            {
                ModelState.AddModelError("PaperFile", "Only .doc and .docx files are allowed for the paper.");
            }
            else
            {
                string directoryPath = Server.MapPath("~/Application/Papers/");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string documentType = "Paper";
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string newFileName = $"{model.Member_Id}_{model.Event_Id}_{documentType}_{timestamp}{extension}";
                string paperPath = Path.Combine(directoryPath, newFileName);
                PaperFile.SaveAs(paperPath);
                model.PaperPath = "/Application/Papers/" + newFileName;

                // Save to DB
                paperDAL.SaveFilePath(new MemberDocumentModel
                {
                    UserID = model.Member_Id,
                    Name = newFileName,
                    DocumentType = documentType,
                    FilePath = model.PaperPath
                });
            }
        }

        // === Plagiarism Report Upload ===
        if (PlagiarismReport != null && PlagiarismReport.ContentLength > 0)
        {
            string extension = Path.GetExtension(PlagiarismReport.FileName).ToLower();
            if (!allowedPlagiarismExtensions.Contains(extension))
            {
                ModelState.AddModelError("PlagiarismReport", "Only .pdf files are allowed for the plagiarism report.");
            }
            else
            {
                string directoryPath = Server.MapPath("~/Application/PlagiarismPolicies/");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string documentType = "PlagiarismReport";
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string newFileName = $"{model.Member_Id}_{model.Event_Id}_{documentType}_{timestamp}{extension}";
                string policyPath = Path.Combine(directoryPath, newFileName);
                PlagiarismReport.SaveAs(policyPath);
                model.PlagiarismPath = "/Application/PlagiarismPolicies/" + newFileName;

                // Save to DB
                paperDAL.SaveFilePath(new MemberDocumentModel
                {
                    UserID = model.Member_Id,
                    Name = newFileName,
                    DocumentType = documentType,
                    FilePath = model.PlagiarismPath
                });
            }
        }




        string result = paperDAL.SubmitPapers(model);

        if (result == "Success")
        {

            int paperId = model.PaperId;

            // ✅ Submit Paper Version (AFTER Paper is saved)
            DAL versionDAL = new DAL();
            string versionMessage = versionDAL.SubmitPaperVersion(
                paperId: paperId,
                eventId: model.Event_Id,
                dateOfSubmission: model.DateOfSubmission,
                path: model.PaperPath,
                complianceReportPath: model.PlagiarismPath
            );

            // ✅ Save Co-authors
            foreach (var coAuthorId in  validCoAuthorIds)
            {
                paperDAL.SaveCoAuthor(new CoAuthorModel
                {
                    PaperId = model.PaperId,
                    MemberId = coAuthorId,
                    IsCorresponding = (coAuthorId == model.Correspondence_Id.ToString())
                });
            }


            return Json(new { success = true, message = "Paper submitted successfully!" });
        }
        else
        {
            return Json(new { success = false, message = "Submission failed: " + result });
        }


    }
}