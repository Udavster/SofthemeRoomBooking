using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index(ApplicationUser userModel)
        {
            return View("Profile", userModel);
        }
    }
}