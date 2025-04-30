//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using INDIACom.App_Cude;
//using System.Web.Mvc;
//using INDIACom.Models;

//namespace INDIACom.Controllers
//{
//    public class FeedbackController : Controller
//    {
//        private DAL dal = new DAL();

//        [HttpGet]
//        public ActionResult SubmitFeedback()
//        {
//            return View();
//        }

//        [HttpPost]
//        public JsonResult SubmitFeedback(FeedbackModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                var errors = ModelState.Values
//                                       .SelectMany(v => v.Errors)
//                                       .Select(e => e.ErrorMessage)
//                                       .ToList();
//                return Json(new { success = false, message = "Validation failed.", errors });
//            }

//            try
//            {
//                // Insert feedback into DB
//                string result = dal.InsertFeedback(model);

//                if (result == "Success")
//                {
//                    return Json(new { success = true, message = "Feedback submitted successfully!" });
//                }
//                else
//                {
//                    return Json(new { success = false, message = "Data insertion failed!" });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Json(new { success = false, message = "An error occurred: " + ex.Message });
//            }
//        }


//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using INDIACom.App_Cude;
using INDIACom.Models;
using System.Data;

namespace INDIACom.Controllers
{
    public class FeedbackController : Controller
    {
        private DAL dal = new DAL();

        [HttpGet]
        public ActionResult SubmitFeedback()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = Session["user"] as MemberModel;
            DataTable dt = dal.GetUserById(user.MemberID);

            if (dt != null && dt.Rows.Count > 0)
            {
                var model = new FeedbackModel
                {
                    Name = dt.Rows[0]["Name"].ToString(),
                    Email = dt.Rows[0]["Email"].ToString(),
                    Mobile = dt.Rows[0]["Mobile"].ToString(),
                    Organization = dt.Rows[0]["Organisation"].ToString()
                };

                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public JsonResult SubmitFeedback(FeedbackModel model)
        {
            if (Session["user"] == null)
            {
                return Json(new { success = false, message = "User not logged in!" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                return Json(new { success = false, message = "Validation failed.", errors });
            }

            try
            {
                var user = Session["user"] as MemberModel;
                DataTable dt = dal.GetUserById(user.MemberID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Overwrite user-related fields just in case someone manipulates them client-side
                    model.Name = dt.Rows[0]["Name"].ToString();
                    model.Email = dt.Rows[0]["Email"].ToString();
                    model.Mobile = dt.Rows[0]["Mobile"].ToString();
                    model.Organization = dt.Rows[0]["Organisation"].ToString();
                }

                string result = dal.InsertFeedback(model);

                if (result == "Success")
                {
                    return Json(new { success = true, message = "Feedback submitted successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Data insertion failed!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }
}

