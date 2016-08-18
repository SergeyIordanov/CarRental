using System.Web.Mvc;

namespace CarRental.WEB.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Manage";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manage_default",
                "Manage/{controller}/{action}/{id}",
                new {controller = "Manage", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}