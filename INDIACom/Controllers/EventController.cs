using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using INDIACom.App_Cude;
using INDIACom.Models;

namespace INDIACom.Controllers
{
    public class EventController : Controller
    {
        private DAL dal = new DAL();

        [HttpGet]
        public ActionResult SubmitEvent()
        {
            var user = Session["user"] as MemberModel;
            if (Session["user"] == null || user.UserTypeId != 1)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public JsonResult SubmitEvent(EventModel model)
        {
            try
            {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                return Json(new { success = false, message = "Validation failed.", errors });
            }

                // Insert feedback into DB
                string result = dal.InsertEvent(model);

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