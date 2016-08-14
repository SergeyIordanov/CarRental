using System.Web.Optimization;

namespace CarRental.WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862 
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                        "~/Scripts/materialize/materialize.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/slider").Include(
                        "~/Scripts/noUiSlider/nouislider.js"));

            bundles.Add(new ScriptBundle("~/bundles/init").Include(
                        "~/Scripts/initializer.js"));           

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/materialize").Include(
                      "~/Content/materialize/css/materialize.css"));

            bundles.Add(new StyleBundle("~/Content/slider").Include(
                      "~/Content/noUiSlider/nouislider*"));
        }
    }
}
