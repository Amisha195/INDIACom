using INDIACom.App_Cude;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INDIACom.Controllers
{
    public class DropdownController : Controller
    {
        // GET: Dropdown

        private readonly DAL dropdownDAL = new DAL();


      
        [HttpPost]
        public JsonResult getDepartment(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "DEPT", "", "", "", "", type);
            return Json(list, 0);
        }

        [HttpPost]
        public JsonResult getEvent(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "EVENT", "", "", "", "", type);
            return Json(list, 0);

        }
        [HttpPost]
        public JsonResult getSession(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            // Call the stored procedure to fetch session data
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "SESSION", "", "", "", "", type);

            return Json(list, 0);
        }
        [HttpPost]
        public JsonResult getTrack(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            // Call the stored procedure to fetch session data
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "TRACK", "", "", "", "", type);

            return Json(list, 0);
        }





    }
}