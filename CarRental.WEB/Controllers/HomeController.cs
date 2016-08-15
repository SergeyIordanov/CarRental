using System.Web.Mvc;

namespace CarRental.WEB.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contacts()
        {
            return View();
        }
    }
}