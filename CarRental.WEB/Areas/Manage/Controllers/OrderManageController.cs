using System.Web.Mvc;

namespace CarRental.WEB.Areas.Manage.Controllers
{
    public class OrderManageController : Controller
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