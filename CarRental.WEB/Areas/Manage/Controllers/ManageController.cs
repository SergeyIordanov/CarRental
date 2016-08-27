using System.Web.Mvc;
using NLog;

namespace CarRental.WEB.Areas.Manage.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class ManageController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Manage/Home page. User: {0}", User.Identity.Name);
            return View();
        }
    }
}