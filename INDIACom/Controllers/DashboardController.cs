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
        [HttpGet]
        public ActionResult UserDashboard()
        {
            var user = Session["user"] as MemberModel;
            if (Session["user"] == null || user.UserTypeId != 3)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult AdminDashboard() 
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