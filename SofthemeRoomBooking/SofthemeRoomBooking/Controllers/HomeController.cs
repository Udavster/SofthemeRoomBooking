using System.Web.Mvc;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(CurrentUserViewModel model)
        {
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
    }
}