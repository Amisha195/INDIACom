using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INDIACom.App_Cude;
using System.Web.Mvc;
using INDIACom.Models;

namespace INDIACom.Controllers
{
    public class FeedbackController : Controller
    {
        private DAL dal = new DAL();

        [HttpGet]
        public ActionResult SubmitFeedback()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitFeedback(FeedbackModel model)
        {
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
                // Insert feedback into DB
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

