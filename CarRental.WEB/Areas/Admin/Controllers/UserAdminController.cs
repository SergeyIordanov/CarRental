using System.Web.Mvc;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserAdminController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}