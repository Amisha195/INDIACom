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
        public JsonResult getTrack(string type = "")
        {
            List<SelectListItem> list = dropdownDAL.GetTracks();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getEvent(string type = "")
        {
        
            List<SelectListItem> list = dropdownDAL.GetEvents();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
       

        [HttpPost]
        public JsonResult getSession(string type = "")
        {
            List<SelectListItem> list = dropdownDAL.GetSessions();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}