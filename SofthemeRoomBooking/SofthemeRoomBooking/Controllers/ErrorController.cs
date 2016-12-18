using System.Web.Mvc;

namespace SofthemeRoomBooking.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;
            HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            HttpContext.Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = 404;
            return View("~/Views/Errors/Http404.cshtml");
        }

        public ActionResult BadRequest()
        {
            //Response.StatusCode = 400;
            return View("~/Views/Errors/Http400.cshtml");
        }

        public ActionResult Error()
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = 500;
            return View("~/Views/Errors/Http500.cshtml");
        }
    }
}