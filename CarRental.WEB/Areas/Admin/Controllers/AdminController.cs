using System.Web.Mvc;
using NLog;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Admin/Home page. User: {0}", User.Identity.Name);
            return View();
        }
    }
}