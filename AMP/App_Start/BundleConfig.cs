using System.Web;
using System.Web.Optimization;

namespace AMP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                //          "~/Content/site.css",
                      "~/Content/typeahead.css"
            //          "~/Content/bootstrap-datetimepicker.css",
            //          "~/Content/less/bootstrap-datetimepicker.less",
            //          "~/Content/less/bootstrap-datetimepicker-build.less",
            //          "~/Content/bootstrap/glyphicons.less"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                      "~/Scripts/typeahead.jquery.js",
                      "~/Scripts/bloodhound.js"
                    ));




            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                "~/Scripts/moment.js",
                "~/Scripts/bootstrap-datetimepicker.js"
          

                    ));

        }
    }
}
