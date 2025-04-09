using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INDIACom.App_Cude;
using System.Web.Mvc;
using INDIACom.Models;
using System.IO;

public class PaperSubmissionController : Controller
{
    private readonly DAL paperDAL = new DAL();
    [HttpGet]
    public ActionResult SubmitPapers()
    {
        return View(); // This will look for Views/PaperSubmission/SubmitPapers.cshtml
    }
    //for verify button
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
        PaperSubmissionModel model = new PaperSubmissionModel
        {
            Title = form["Title"],
            DateOfSubmission = DateTime.Now,
            Event_Id = Convert.ToInt32(form["Event_Id"]),
            Track_Id = Convert.ToInt32(form["Track_Id"]),
            Session_Id = Convert.ToInt32(form["Session_Id"]),
            Event_Name = form["Event_Name"],
            Track_Name = form["Track_Name"],
            Session_Name = form["Session_Name"],
            Member_Id = Convert.ToInt32(form["Authors[0].MemberID"]),  // First author as main
            Correspondence_Id = Convert.ToInt32(form["CorrespondingAuthorID"]),
            Co_Authors_Id = form["co_authors_id"]
        };
        //for co_authors

        List<string> coAuthorIds = new List<string>();
        for (int i = 0; i < 8; i++) // Assuming max 8 co-authors
        {
            string authorId = form[$"Authors[{i}].MemberID"];
            if (!string.IsNullOrEmpty(authorId))
            {
                coAuthorIds.Add(authorId);
            }
        }
        model.Co_Authors_Id = string.Join(",", coAuthorIds);


        if (PaperFile != null && PaperFile.ContentLength > 0)
        {
            string directoryPath = Server.MapPath("~/Application/Papers/");

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Create a unique filename using timestamp
            string uniqueFileName = Path.GetFileNameWithoutExtension(PaperFile.FileName) +
                                    "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                    Path.GetExtension(PaperFile.FileName);

            string paperPath = Path.Combine(directoryPath, uniqueFileName);
            PaperFile.SaveAs(paperPath);

            model.PaperPath = "/Application/Papers/" + uniqueFileName;
        }

        // For Plagiarism Report
        if (PlagiarismReport != null && PlagiarismReport.ContentLength > 0)
        {
            string directoryPath = Server.MapPath("~/Application/PlagiarismPolicies/");

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Create a unique filename using timestamp
            string uniquePolicyName = Path.GetFileNameWithoutExtension(PlagiarismReport.FileName) +
                                      "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                      Path.GetExtension(PlagiarismReport.FileName);

            string policyPath = Path.Combine(directoryPath, uniquePolicyName);
            PlagiarismReport.SaveAs(policyPath);

            model.PlagiarismPath = "/Application/PlagiarismPolicies/" + uniquePolicyName;
        }

        DAL paperDAL = new DAL();
        string result = paperDAL.SubmitPapers(model);

        if (result == "Success")
        {
            return Json(new { success = true, message = "Paper submitted successfully!" });
        }
        else
        {
            return Json(new { success = false, message = "Submission failed: " + result });
        }

       

       

    }
}


