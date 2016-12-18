using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SofthemeRoomBooking.Controllers
{
    public abstract class ErrorCatchingControllerBase : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            this.RedirectToAction("NotFound","Error").ExecuteResult(this.ControllerContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];

            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            try
            {
                var exception = model.Exception;
                if ((exception is ArgumentException) && (exception.Source == "System.Web.Mvc"))
                {
                    this.RedirectToAction("BadRequest", "Error").ExecuteResult(this.ControllerContext);
                    return;
                }
            }
            catch (Exception) { }

            this.RedirectToAction("Error", "Error").ExecuteResult(this.ControllerContext);
        }
    }
}