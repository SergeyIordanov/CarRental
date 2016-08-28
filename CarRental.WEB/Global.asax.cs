using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace CarRental.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            Logger.Info("---------- Applicaton starts ----------");
            AreaRegistration.RegisterAllAreas();
            Logger.Debug("Areas registered");
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Logger.Debug("Filters registered");
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Logger.Debug("Routes registered");
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Logger.Debug("Bundles registered");
        }
    }
}
