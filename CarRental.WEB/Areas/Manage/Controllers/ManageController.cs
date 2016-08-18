using System.Web.Mvc;

namespace CarRental.WEB.Areas.Manage.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class ManageController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}