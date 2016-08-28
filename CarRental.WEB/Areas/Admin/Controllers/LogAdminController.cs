using System.Web.Mvc;
using CarRental.BLL.Interfaces;
using NLog;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class LogAdminController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly IRentService _rentService;

        public LogAdminController(IRentService serv)
        {
            _rentService = serv;
        }

        [HttpGet]
        public ActionResult Index()
        {
            Logger.Debug("Request to Admin/Log. User: {0}", User.Identity.Name);
            return View(model: _rentService.GetCurrentLog(Server.MapPath(@"~/App_Data/Logs/Info")));
        }
    }
}