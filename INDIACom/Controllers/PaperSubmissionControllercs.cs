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
    private readonly DAL _paperDAL = new DAL();
    [HttpGet]
    public ActionResult SubmitPapers()
    {
        return View(); // This will look for Views/PaperSubmission/SubmitPapers.cshtml
    }


   
    //[HttpPost]
    //public ActionResult SubmitPapers(PaperSubmissionModel model)
    //{
    //    // Ensure files are uploaded properly
    //    if (Request.Files["PaperDocument"] == null || Request.Files["PlagiarismReport"] == null)
    //    {
    //        ViewBag.Message = "Both paper and plagiarism report must be uploaded.";
    //        return View(model);
    //    }

    //    HttpPostedFileBase paperFile = Request.Files["PaperDocument"];
    //    HttpPostedFileBase plagFile = Request.Files["PlagiarismReport"];

    //    if (paperFile == null || paperFile.ContentLength == 0 || plagFile == null || plagFile.ContentLength == 0)
    //    {
    //        ViewBag.Message = "Both paper and plagiarism report must be uploaded.";
    //        return View(model);
    //    }

    //    string paperPath = "";
    //    string plagiarismPath = "";

    //    string paperFolder = Server.MapPath("~/Uploads/Papers/Documents/");
    //    string plagFolder = Server.MapPath("~/Uploads/Papers/PlagiarismReports/");

    //    try
    //    {
    //        if (!Directory.Exists(paperFolder)) Directory.CreateDirectory(paperFolder);
    //        if (!Directory.Exists(plagFolder)) Directory.CreateDirectory(plagFolder);

    //        // Save Paper Document
    //        string paperFileName = Guid.NewGuid() + Path.GetExtension(paperFile.FileName);
    //        paperPath = Path.Combine(paperFolder, paperFileName);
    //        paperFile.SaveAs(paperPath);

    //        // Save Plagiarism Report
    //        string plagFileName = Guid.NewGuid() + Path.GetExtension(plagFile.FileName);
    //        plagiarismPath = Path.Combine(plagFolder, plagFileName);
    //        plagFile.SaveAs(plagiarismPath);

    //        // Store paths in model
    //        model.paper_path = paperPath;
    //        model.plagiarism_path = plagiarismPath;

    //        // Insert into DB
           
    //            bool isInserted = _paperDAL.SubmitPapers(model);

    //            if (isInserted)
    //            {
    //                ViewBag.Message = "Paper submitted successfully!";
    //            }
    //            else
    //            {
    //                ViewBag.Message = "Error submitting the paper. Check logs for details.";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ViewBag.Message = "Exception: " + ex.Message; // ✅ Show exact error
    //        }


    //        return View(model);
    //    }

     }
    


