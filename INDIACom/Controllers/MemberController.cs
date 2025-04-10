using INDIACom.App_Cude;
using System.Web.Mvc;
using System;
using System.Linq;
using INDIACom.Models;
using System.Data;
using System.IO;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;
using System.Web.Helpers;


namespace INDIACom.Controllers
{
    public class MemberController : Controller
    {
        private DAL dal = new DAL();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

     [HttpPost]
        public JsonResult SubmitRegister(MembersModel model, HttpPostedFileBase file)
        {


            var pwdValidationResult = PwdValid(model.Password, model.ConfirmPassword);

            // Extract the JSON response
            dynamic resultData = pwdValidationResult.Data;
            if (!(bool)resultData.success)
            {
                return Json(resultData); // Return the validation error
            }


            try
            {
               string Email = model.Email;
               string Name = model.Name;
                long Mobile = model.Mobile;
                 
            if(model.OrganisationName == null)
            {
                string org = AddOrganisation(model);

                if (org == "Success") model.OrganisationName = model.OrgName;
                else model.OrganisationName = null;
            }
                // Insert register user_details into DB
                string result = dal.InsertUserDetails(model);

                if (result == "Success")
                {

                    long id = dal.GetMemberID(Email);
                    if (id > 0 && file != null)
                    {
                       return UploadFile(file, id, Name);
                    }
                    else if(id == 0) 
                    {
                        return Json(new { success = false, message = "Member ID retrieval failed!" });
                    }
                    else
                    {
                        return Json(new { success = true, message = "Registerd Successfully" });

                    }
                }
                else
                {
                    return Json(new { success = false, message = "Registration failed!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public JsonResult UploadFile(HttpPostedFileBase file, long userId, string name)
        {
            if (file == null || file.ContentLength <= 0)
            {
                return Json(new { success = false, message = "Please select a file." });
            }

            try
            {
                string documentType = "Resume"; // Hardcoded for now
                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower() != ".pdf")
                {
                    return Json(new { success = false, message = "Only PDF files are allowed." });
                }

                // Creating the filename
                string newFileName = $"{userId}_{name}_{documentType}{extension}";
                string uploadPath = Server.MapPath("~/UploadedFiles/");

                // Ensure the directory exists
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Full file path
                string filePath = Path.Combine(uploadPath, newFileName);
                file.SaveAs(filePath);

                // Save file path to DB
                DAL dal = new DAL();
                MemberDocumentModel doc = new MemberDocumentModel
                {
                    UserID = userId,
                    Name = name,
                    DocumentType = documentType,
                    FilePath = filePath
                };
                string saveResult = dal.SaveFilePath(doc);

                if (saveResult == "Success")
                {
                    return Json(new { success = true, message = "File uploaded successfully!", filePath = "/UploadedFiles/" + newFileName });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save file path in database." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        public string AddOrganisation(MembersModel org)
        {
            if (org == null)
            {
                return "failed";
            }
            
            string result =  dal.AddOrganisation(org);

            if (result == "Success")
            {
                return result;
            }
            else return null;
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = Session["user"] as MemberModel;
            DataTable dt = dal.GetUserById(user.MemberID); // Custom DAL method

            if (dt != null && dt.Rows.Count > 0)
            {
                MemberModel model = new MemberModel
                {
                    MemberID = long.Parse(dt.Rows[0]["member_id"].ToString()),
                    Name = dt.Rows[0]["Name"].ToString(),
                    Email = dt.Rows[0]["Email"].ToString(),
                    Category = dt.Rows[0]["Category"].ToString(),
                    IEEE_No = dt.Rows[0]["IEEE_No"].ToString(),
                    CSI_No = dt.Rows[0]["CSI_No"].ToString(),
                    Event = dt.Rows[0]["Event"].ToString(),
                    Password = dt.Rows[0]["Password"].ToString(),
                    Biodata = dt.Rows[0]["Bio_data_path"].ToString(),
                    OrganisationName = dt.Rows[0]["Organisation"].ToString(),
                    Salutation = dt.Rows[0]["Salutation"].ToString(),
                    CountryID = dt.Rows[0]["CountryID"].ToString(),
                    Country = dt.Rows[0]["Country"].ToString(),
                    Address = dt.Rows[0]["Address"].ToString(),
                    Mobile = dt.Rows[0]["Mobile"].ToString(),
                    Pincode= dt.Rows[0]["Pincode"].ToString(),


                };

                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]

        public JsonResult UpdateProfile(MemberModel model, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (file != null)
                    {
                        return UploadFile(file, model.MemberID, model.Name);
                    }
                    if (model.OrganisationName == null)
                    {
                        string org = AddOrganisation(model);

                        if (org == "Success") model.OrganisationName = model.OrgName;
                        else model.OrganisationName = null;
                    }
                    bool isUpdated = dal.UpdateUserProfile(model);

                    if (isUpdated)
                    {
                        Session["user"] = model; // Refresh session
                        return Json(new { success = true, message = "Profile updated successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to update profile." });
                    }
                }

                // If model state is invalid
                return Json(new { success = false, message = "Invalid form data." });
            }
            catch (Exception ex)
            {
                // You can also log the exception here
                return Json(new { success = false, message = "An error occurred while updating the profile.", error = ex.Message });
            }
        }

        public string AddOrganisation(MemberModel org)
        {
            if (org == null)
            {
                return "failed";
            }

            string result = dal.AddOrganisation(org);

            if (result == "Success")
            {
                return result;
            }
            else return null;
        }



        public JsonResult PwdValid(string pwd, string confirmPwd)
        {
            
            try
            {
                if (pwd != confirmPwd)
                {
                    return Json(new { success = false, message = "Password and Confirm Password do not match!" });
                }
                else if (pwd.Length < 8 || pwd.Length > 16)
                {
                    return Json(new { success = false, message = "Password must be between 8 and 16 characters." });
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(pwd, @"^[a-zA-Z0-9!@#$%^&*]+$"))
                {
                    return Json(new { success = false, message = "Password must contain only letters, digits, and special characters !@#$%^&*" });
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(pwd, @"[a-z]"))
                {
                    return Json(new { success = false, message = "Password must contain at least one lowercase letter." });
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(pwd, @"[A-Z]"))
                {
                    return Json(new { success = false, message = "Password must contain at least one uppercase letter." });
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(pwd, @"\d"))
                {
                    return Json(new { success = false, message = "Password must contain at least one digit." });
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(pwd, @"[!@#$%&*?()]"))
                {
                    return Json(new { success = false, message = "Password must contain at least one of the following: !@#$%&*?()" });
                }

                return Json(new { success = true });
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred during password validation." });
            }
        }


       


    }
}