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
            return View();
        }

        public ActionResult AdminDashboard() 
        {
            return View();
        }
    }
}