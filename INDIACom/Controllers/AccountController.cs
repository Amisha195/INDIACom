using INDIACom.App_Cude;
using System.Web.Mvc;
using System;
using System.Linq;
using INDIACom.Models;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Web.Security;


namespace INDIACom.Controllers
{
    public class AccountController : Controller
    {
        private DAL dal = new DAL();

        static string Email = "";

        private string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        [HttpGet]
        public ActionResult Login()
        {

            LoginUserModel loginModel = new LoginUserModel();
            string salt = CreateSalt(5);
            Session["salt"] = salt.ToString();
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Check_Details(LoginUserModel user)
        {
            string str = "";
            try
            {
                if (Session["salt"] == null)
                {
                    str = "Error ! Sorry Page Session timeout, Please Refresh the Page";
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                DataTable dt = new DAL().checkCredentials(user);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Msg"].ToString().Trim() == "Success")
                        {
                            string DBPass = dt.Rows[0]["Password"].ToString().ToLower();
                            if (user.Password.ToLower() == DBPass)
                            {
                                user.LoggedInUser = dt.AsEnumerable().Select(m => new MemberModel
                                {
                                    MemberID = long.Parse(m["member_id"].ToString()),
                                    Name = m["Name"].ToString(),
                                    Email = m["Email"].ToString(),
                                    Category = m["Category"].ToString(),
                                    IEEE_No = m["IEEE_No"].ToString(),
                                    CSI_No = m["CSI_No"].ToString(),
                                    url = m["URL"].ToString(),
                                    UserPassword = m["Password"].ToString(),
                                    Event = m["Event"].ToString(),
                                    UserType = m["UserType"].ToString(),
                                    UserTypeId = short.Parse(m["UserTypeId"].ToString())

                                }).FirstOrDefault();
                                /*str = "success";*/
                                FormsAuthentication.SetAuthCookie(user.Email, false);

                                Session["user"] = user.LoggedInUser;


                                if (user.LoggedInUser.UserTypeId == 1)
                                {
                                    return Json(new { status = "success", message = "", url = "/Dashboard/AdminDashboard", showPdf = "no" }, 0);
                                }
                                else if (user.LoggedInUser.UserTypeId == 2)
                                {
                                    return Json(new { status = "success", message = "", url = "/Dashboard/FacultyDashboard", showPdf = "no" }, 0);
                                }
                                else if (user.LoggedInUser.UserTypeId == 3)
                                {
                                    return Json(new { status = "success", message = "", url = "/Dashboard/UserDashboard", showPdf = "no" }, 0);
                                }

                            }
                            else
                            {
                                str = "Error ! Invalid Username or Password";
                            }
                        }
                        else
                        {
                            str = dt.Rows[0]["Msg"].ToString().Trim();
                        }
                    }

                    else
                    {
                        str = dt.Rows[0]["Msg"].ToString().Trim();
                    }
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                str = "Error ! Something went Wrong";
            }
            return Json(str, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();
                Session.Abandon();

                return RedirectToAction("Login", "Account");
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            finally
            {
                Session.Abandon();
            }
        }



        [HttpGet]
        public ActionResult retrievePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RetrievePwd(long id)
        {
            if (id == 0)
            {
                return Json(new { success = false, message = "Enter a member id" });
            }
            else
            {
                // Retrieve user data from the database
                DataTable dt = dal.GetUserById(id);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Map the data from the DataTable to the RetrieveModel
                    RetrieveModel model = new RetrieveModel
                    {
                        Member_Id = long.Parse(dt.Rows[0]["member_id"].ToString()),
                        Password = dt.Rows[0]["Password"].ToString()
                    };

                    // Return the password in the response
                    return Json(new { success = true, password = model.Password });
                }
                else
                {
                    return Json(new { success = false, message = "Member not found!" });
                }
            }
        }

        [HttpGet]
        public ActionResult retrieveMID()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RetrieveID(string email)
        {
            if (email == null)
            {
                return Json(new { success = false, message = "Enter a valid email" });
            }
            else
            {
                // Retrieve user data from the database
                long ID = dal.GetMemberID(email);

                if (ID != 0)
                {
                    // Map the data from the DataTable to the RetrieveModel
                    RetrieveModel model = new RetrieveModel
                    {
                        Member_Id = ID
                 
                    };

                    // Return the password in the response
                    return Json(new { success = true, ID = model.Member_Id });
                }
                else
                {
                    return Json(new { success = false, message = "Member not found!" });
                }
            }
        }
    }
}