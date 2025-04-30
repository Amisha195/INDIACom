using System.Collections.Generic;
using System.Web.Mvc;
using INDIACom.Models;
using INDIACom.App_Cude;

namespace INDIACom.Controllers
{
    public class PaperAdminController : Controller
    {
        public ActionResult SubmitPaperAdmin()
        {
            DAL dal = new DAL();
            List<PaperModel> papers = dal.GetAllPapers();
            return View(papers); // Goes to Views/Admin/SubmitPaperAdmin.cshtml
        }
    }
}
