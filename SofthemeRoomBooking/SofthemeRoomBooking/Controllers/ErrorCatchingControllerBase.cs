using System;
using System.Web;
using System.Web.Mvc;
using Logger = SofthemeRoomBooking.Services.Logger;

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

            var exception = model.Exception;

            string logMessage = String.Format("{0}: {1}", DateTime.Now, exception.ToString());
            Logger.Log.Error(logMessage);

            HttpException httpException = exception as HttpException;
            if (httpException != null)
            {
                throw httpException;
            }

            if ((exception is ArgumentException) && (exception.Source == "System.Web.Mvc"))
            {
                this.RedirectToAction("BadRequest", "Error").ExecuteResult(this.ControllerContext);
                return;
            }

            this.RedirectToAction("Error", "Error").ExecuteResult(this.ControllerContext);
        }
    }
}