using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            return View("Profile");
        }
    }
}