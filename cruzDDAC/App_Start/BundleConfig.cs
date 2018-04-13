using System.Web;
using System.Web.Optimization;

namespace cruzDDAC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                           "~/Scripts/jquery.js",
                           "~/Scripts/bootstrap.min.js",
                           "~/Scripts/jquery.parallax.js",
                           "~/Scripts/owl.carousel.min.js",
                           "~/Scripts/smoothscroll.js",
                            "~/Scripts/wow.min.js",
                           "~/Scripts/custom.js",
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                     "~/Scripts/audioplayer.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/jquery.mobile.custom.min.js",
                        "~/Scripts/imagesloaded.js",
                         "~/Scripts/fws3.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/animate.css",
                         "~/Content/font-awesome.min.css",
                         "~/Content/owl.theme.css",
                          "~/Content/owl.carousel.css",
                     "~/Content/style1.css",
                      "~/Content/audioplayer.css"

                     ));

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                  "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/slide").Include(
                         "~/Scripts/slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
          "~/Scripts/jquery-ui.min.js"));
            //css  
            bundles.Add(new StyleBundle("~/Content/cssjqryUi").Include(
                   "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/boostrapdatetime").Include(
                  "~/Content/bootstrap-datetimepicker.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/boostrapdatetime").Include(
                   "~/Scripts/moment.js",
                   "~/Scripts/bootstrap-datetimepicker.js"));
            //aaa
            bundles.Add(new StyleBundle("~/Content/backanimated").Include(
                 "~/Content/BackAnimated.css"));
            bundles.Add(new ScriptBundle("~/bundles/backanimated").Include(
                 
                   "~/Scripts/BackAnimatedJS.js"));
        }
    }
}
