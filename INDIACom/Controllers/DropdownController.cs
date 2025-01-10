using INDIACom.App_Cude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INDIACom.Controllers
{
    public class DropdownController : Controller
    {
        // GET: Dropdown
        [HttpPost]
        public JsonResult getDepartment(string type = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            CommonMethod.bindDropDownHnGrid("Proc_Common", list, "DEPT", "", "", "", "", type);
            return Json(list, 0);
        }
    }
}