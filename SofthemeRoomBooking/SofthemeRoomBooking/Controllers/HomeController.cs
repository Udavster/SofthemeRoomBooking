using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Services.Contracts;

namespace SofthemeRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProfileService _profileService;

        public HomeController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = _profileService.GetLayoutUserViewModelById(User.Identity.GetUserId());
                if (model != null)
                {
                    ViewBag.AdminRole = _profileService.IsAdmin(User.Identity.GetUserId());
                                    
                    return PartialView("Menu", model);
                }
            }
            
            return PartialView("Menu", null);
        }
    }
}