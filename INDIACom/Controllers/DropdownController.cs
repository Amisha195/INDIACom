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

        [HttpPost]
        public JsonResult getEvent(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "EVENT", "", "", "", "", type);
            return Json(list, 0);

        }


        //[HttpPost]
        //public JsonResult getTrack(string type = "")
        //{
        //    List<SelectListItem> list = dropdownDAL.GetTracks();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult getEvent(string type = "")
        //{

        //    List<SelectListItem> list = dropdownDAL.GetEvents();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //public JsonResult getSession(string type = "")
        //{
        //    List<SelectListItem> list = dropdownDAL.GetSessions();
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult getCountries(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "COUNTRY", "", "", "", "", type);
            return Json(list, 0);

        }

        public JsonResult getStates(string countryId, string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "STATE", countryId, "", "", "", type);
            return Json(list, 0);

        }

        public JsonResult getCities(string countryId, string stateId, string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "CITY", countryId, stateId, "", "", type);
            return Json(list, 0);

        }

        public JsonResult getOrg(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "ORG", "", "", "", "", type);
            return Json(list, 0);

        }

    }
}