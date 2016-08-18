using System.Web.Mvc;

namespace CarRental.WEB.Areas.Manage.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult NewOrders()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CurrentOrders()
        {
            return View();
        }
    }
}