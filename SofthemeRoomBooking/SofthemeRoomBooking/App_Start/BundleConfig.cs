using System.Web.Optimization;

namespace SofthemeRoomBooking
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Validation/jquery.validate*",
                        "~/Scripts/Validation/validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/Login").Include(
                         "~/Scripts/jquery-{version}.js",
                       "~/Scripts/Validation/jquery.validate*",
                        "~/Scripts/Validation/validation.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/calendar").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/Calendar/calendarDatepicker.js",
                        "~/Scripts/Calendar/calendarEvents.js",
                        "~/Scripts/Calendar/commonCalendar.js"
                        ));

            //
            bundles.Add(new StyleBundle("~/bundles/Layout/css").Include(
                      //"~/Content/font-awesome.css",
                      "~/fonts/font-awesome.css",
                      "~/Content/Styles/validation.css",
                      "~/Content/Styles/common.css",
                      "~/Content/Styles/input_styles.css",
                      "~/Content/Styles/layout.css",
                      "~/Content/Styles/popup-confirmation.css",
                      "~/Content/Styles/map.css",
                      "~/Content/Styles/room.css",
                      "~/Content/Styles/users.css",
                      "~/Content/Styles/pagination.css"));

            bundles.Add(new StyleBundle("~/bundles/Login/css").Include(
                      "~/Content/Styles/login.css",
                      "~/Content/Styles/input_styles.css",
                      "~/Content/Styles/validation.css",
                      "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/feedback").Include(
                "~/Content/Styles/feedback.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/users").Include(
                "~/Content/Styles/users.css",
                "~/Content/Styles/pagination.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/profile").Include(
                "~/Content/Styles/profile.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/calendar").Include(
                "~/Content/Styles/Calendar/calendarDatepicker.css",
                "~/Content/Styles/Calendar/calendarEvents.css",
                "~/Content/Styles/Calendar/calendarCommon.css"
                ));
        }
    }
}
