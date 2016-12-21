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
            bundles.Add(new ScriptBundle("~/bundles/eventInfo").Include(
                         "~/Scripts/Event/eventInfo.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/datetimepickers").Include(
                        "~/Scripts/DateTimePickers/datepicker.js",
                        "~/Scripts/DateTimePickers/timepicker.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/event-create").Include(
                        "~/Scripts/Event/event-common.js",
                        "~/Scripts/Event/event-create.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/event-edit").Include(
                        "~/Scripts/Event/event-common.js",
                        "~/Scripts/Event/event-edit.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/event-index").Include(
                        "~/Scripts/Event/event-common.js",
                        "~/Scripts/Event/event-index.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/event-participants").Include(
                        "~/Scripts/Event/event-participants.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/event-details").Include(
                        "~/Scripts/event-details-popup.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/shedulerEvents").Include(
                        "~/Scripts/Room/schedulerEvents.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/room-general").Include(
                        "~/Scripts/Room/room-general.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/room-home").Include(
                        "~/Scripts/Room/roomHome.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/equipmentAdmin").Include(
                        "~/Scripts/Room/equipmentValidation.js"
                        ));

            bundles.Add(new StyleBundle("~/bundles/Layout/css").Include(
                      //"~/Content/font-awesome.css",
                      //"~/fonts/font-awesome.css",
                      "~/Content/Styles/validation.css",
                      "~/Content/Styles/common.css",
                      "~/Content/Styles/input_styles.css",
                      "~/Content/Styles/layout.css",
                      "~/Content/Styles/popup-confirmation.css",
                      "~/Content/Styles/map.css",
                      "~/Content/Styles/room.css",
                      "~/Content/Styles/equipment.css",
                      "~/Content/Styles/roombooking.css",
                      "~/Content/Styles/weekScheduler.css",
                      "~/Content/Styles/users.css",
                      "~/Content/Styles/pagination.css",
                      "~/Content/Styles/Event/index.css",
                      "~/Content/Styles/Event/editevent.css",
                      "~/Content/Styles/Event/eventcommon.css",
                      "~/Content/Styles/Event/eventcreate.css",
                      "~/Content/Styles/Event/eventedit.css",
                      "~/Content/Styles/Event/eventinfo-private.css",
                      "~/Content/Styles/Event/eventinfo-public.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/Login/css").Include(
                      "~/Content/Styles/login.css",
                      "~/Content/Styles/input_styles.css",
                      "~/Content/Styles/validation.css"
                      //"~/Content/font-awesome.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/Room/css").Include(
                      "~/Content/Styles/Room/roomView.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/Styles/feedback").Include(
                "~/Content/Styles/feedback.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/users").Include(
                "~/Content/Styles/users.css",
                "~/Content/Styles/pagination.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/profile").Include(
                "~/Content/Styles/profile.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/errors").Include(
               "~/Content/Styles/errors.css"));

            bundles.Add(new StyleBundle("~/bundles/Styles/calendar").Include(
                "~/Content/Styles/Calendar/calendarDatepicker.css",
                "~/Content/Styles/Calendar/calendarEvents.css",
                "~/Content/Styles/Calendar/calendarCommon.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/Styles/datetimepickers").Include(
                        "~/Content/Styles/DateTimePickers/datepicker.css",
                        "~/Content/Styles/DateTimePickers/timepicker.css"
                        ));
        }
    }
}
