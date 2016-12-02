using System.Web.Mvc;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;

namespace SofthemeRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(LayoutUserViewModel model)
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
    }
}