using System.Web.Mvc;

namespace CarRental.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderAdminController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}