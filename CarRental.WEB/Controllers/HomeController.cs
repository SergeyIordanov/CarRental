using System.Web.Mvc;
using NLog;

namespace CarRental.WEB.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Home page.");
            return View();
        }

        [HttpGet]
        public ActionResult Contacts()
        {
            Logger.Debug("Request to Home page.");
            return View();
        }
    }
}