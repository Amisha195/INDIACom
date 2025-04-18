using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using INDIACom.Models;
using System.IO;
using INDIACom.App_Cude;

namespace INDIACom.Controllers
{
    public class CategoryVerificationController : Controller
    {
        private DAL dal = new DAL(); 

        // GET: Admin (Displays uploaded PDFs)
        public ActionResult Admin()
        {
            List<MembersDocumentsModel> documents = dal.GetMemberDocuments();
            return View(documents);
        }

        // GET: Document Verification Page
        public ActionResult DocumentVerification()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = Session["user"] as MemberModel;
            if (user != null)
            {
                long memberId = user.MemberID; 
            }

            return View();
        }

        // POST: Handles multiple file uploads
        [HttpPost]
        public ActionResult UploadVerificationDocuments(MembersDocumentsModel model, HttpPostedFileBase fileMemCat, HttpPostedFileBase fileProfBody, HttpPostedFileBase fileOtherDocs)
        {
            if (Session["user"] == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            // Grab the current user from session
            var user = Session["user"] as MemberModel;
            if (user == null)
            {
                return Json(new { success = false, message = "Unable to retrieve user information." });
            }

            long memberId = user.MemberID; // Get the MemberID from the session

            if (fileMemCat == null || fileProfBody == null)
            {
                return Json(new { success = false, message = "Institution Card and Professional Body ID Card are required." });
            }

            string uploadDir = Server.MapPath("~/Content/uploadedFiles/");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            try
            {
                // Save Institution Card
                if (fileMemCat != null && fileMemCat.ContentLength > 0)
                {
                    string fileName = $"{memberId}_InstitutionCard_{Path.GetFileNameWithoutExtension(fileMemCat.FileName)}{Path.GetExtension(fileMemCat.FileName)}";
                    string filePath = Path.Combine(uploadDir, fileName);
                    fileMemCat.SaveAs(filePath);
                    model.InstitutionCardPath = fileName;
                }

                // Save Professional Body ID Card
                if (fileProfBody != null && fileProfBody.ContentLength > 0)
                {
                    string fileName = $"{memberId}_MembershipProof_{Path.GetFileNameWithoutExtension(fileProfBody.FileName)}{Path.GetExtension(fileProfBody.FileName)}";
                    string filePath = Path.Combine(uploadDir, fileName);
                    fileProfBody.SaveAs(filePath);
                    model.ProfBodyIDCardPath = fileName;
                }

                // Save Other Documents (optional)
                if (fileOtherDocs != null && fileOtherDocs.ContentLength > 0)
                {
                    string fileName = $"{memberId}_OtherDocument_{Path.GetFileNameWithoutExtension(fileOtherDocs.FileName)}{Path.GetExtension(fileOtherDocs.FileName)}";
                    string filePath = Path.Combine(uploadDir, fileName);
                    fileOtherDocs.SaveAs(filePath);
                    model.PassportPath = fileName; // You may want to rename this field if it's for other types of documents
                }

                // Set MemberID in the model before saving
                model.MemberID = (int)memberId;

                // Save to DB using DAL
                string result = dal.InsertMemberDocument(model);

                if (result == "Success")
                {
                    return Json(new { success = true, message = "Documents uploaded successfully." });
                }
                else
                {
                    return Json(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
