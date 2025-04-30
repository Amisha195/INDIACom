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