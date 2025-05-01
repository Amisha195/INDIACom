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

namespace INDIACom.Controllers
{
    public class DashboardController : Controller
    {
        private DAL dal = new DAL();
        [HttpGet]
        public ActionResult UserDashboard()
        {
            var user = Session["user"] as MemberModel;
            DataTable dt = dal.GetUserById(user.MemberID);
            if (Session["user"] == null || user.UserTypeId != 3)
            {
                return RedirectToAction("Login", "Account");
            }
           else if (dt != null && dt.Rows.Count > 0)
            {
                MemberModel model = new MemberModel
                {
                    Name = dt.Rows[0]["Name"].ToString()
                };
                return View(model);
            }
           else 
            return View();

        }

        public ActionResult dashboard()
        {
            var user = Session["user"] as MemberModel;
            if (Session["user"] == null || user.UserTypeId != 1 )
            {
                return RedirectToAction("Login", "Account");
            }

                 return View();
            
        }

        public ActionResult dashboard()
        {
            return View();
        }
    }
}