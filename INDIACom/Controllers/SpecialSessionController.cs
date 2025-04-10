using INDIACom.App_Cude;
using INDIACom.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace INDIACom.Controllers
{
    public class SpecialSessionController : Controller
    {
        private DAL dal = new DAL();
        [HttpGet]
        public ActionResult SpecialSession()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = Session["user"] as MemberModel;
            DataTable dt = dal.GetUserById(user.MemberID);

            if (dt != null && dt.Rows.Count > 0)
            {
                SpecialSessionModel model = new SpecialSessionModel
                {
                    MemberID = user.MemberID // 👈 Pass the MemberID to the view model
                };

            

                return View(model);

            }
            return RedirectToAction("Login", "Account");

        }





        [HttpGet]
        public ActionResult SpecialSession2()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = Session["user"] as MemberModel;
            DataTable dt = dal.GetUserById(user.MemberID);

            if (dt != null && dt.Rows.Count > 0)
            {
                MemberModel model = new MemberModel
                {
                    Name = dt.Rows[0]["Name"].ToString()
                };

                return View(model);

            }
            return RedirectToAction("Login", "Account");

        }



        [HttpGet]
        public ActionResult SpecialSessionStatic()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = Session["user"] as MemberModel;
            DataTable dt = dal.GetUserById(user.MemberID);

            if (dt != null && dt.Rows.Count > 0)
            {
                MemberModel model = new MemberModel
                {
                    MemberID = long.Parse(dt.Rows[0]["member_id"].ToString()),
                    Biodata = dt.Rows[0]["Bio_data_path"] == DBNull.Value ? null : dt.Rows[0]["Bio_data_path"].ToString()
                };
                if (model.Biodata == null) {
                    return RedirectToAction("SpecialSession2", "SpecialSession");
                }

                return RedirectToAction("SpecialSession", "SpecialSession");

            }
            return RedirectToAction("Login", "Account");

        }

     






        [HttpPost]
        public JsonResult SpecialSession(SpecialSessionModel ss)
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
               

                string result = dal.InsertSession(ss);
                if (result == "Success")
                {
                    return Json(new { success = true, message = "Session Inserted Successfully!!" });
                }
                else
                {
                    return Json(new { success = false, message = "Data Insertion failed!!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred:" + ex.Message });
            }
        }
    }
}