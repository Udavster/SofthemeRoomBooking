using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;

namespace SofthemeRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult Menu()
        {
            //db
            //(User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == "firstname")
            return PartialView();
        }
    }
}