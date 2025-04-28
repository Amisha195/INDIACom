//using System;
//using System.Collections.Generic;
//using System.Web.Mvc;
//using INDIACom.Models;
//using System.Data;
//using System.Linq;
//using INDIACom.App_Cude;

//namespace INDIACom.Controllers
//{
//    public class UserInfoController : Controller
//    {
//        // GET: Member/UserInfoAdmin
//        public ActionResult UserInfo()
//        {
//            List<UserInfoModel> membersList = new List<UserInfoModel>();

//            try
//            {
//                DAL dal = new DAL(); // DAL class you'll create next
//                membersList = dal.GetAllMembers();
//            }
//            catch (Exception ex)
//            {
//                // Optionally log error or display user-friendly message
//                ViewBag.Error = "Error fetching member data: " + ex.Message;
//            }

//            return View(membersList);
//        }
//    }
//}


////////////////////

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using INDIACom.Models;
using System.Data;
using System.Linq;
using INDIACom.App_Cude;

namespace INDIACom.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: Member/UserInfoAdmin
        // existing list page  -----------------------------
        public ActionResult UserInfo(int page = 1, int pageSize = 10)
        {
            DAL dal = new DAL();
            var allUsers = dal.GetAllMembers();        // already filtered columns
            var usersToShow = allUsers.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalUsers = allUsers.Count;
            return View(usersToShow);
        }

        // NEW detail page  -------------------------------
        public ActionResult UserDetails(int id)          // id comes from link
        {
            DAL dal = new DAL();
            UserInfoModel user = dal.FetchUserById(id);    // implement in DAL

            if (user == null) return HttpNotFound();

            return View(user);                           // goes to Views/UserInfo/UserDetails.cshtml
        }

    }
}