using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using INDIACom.App_Cude;
using System.Threading;

namespace INDIACom
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            StartEventCleanupScheduler();
        }
      
        private static Timer timer;
        private void StartEventCleanupScheduler()
        {
            timer = new Timer(DeleteExpiredEvents, null, 0, 60000); // Runs every 1 minutes
        }

        private void DeleteExpiredEvents(object state)
        {
            try
            {
                DAL dropdownDAL = new DAL();
                dropdownDAL.DeleteExpiredEvents();
            }
            catch (Exception ex)
            {
                // Log error if needed
            }
        }
    }
}
