using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using SofthemeRoomBooking.Services.Models;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Services.Contracts;

namespace SofthemeRoomBooking.Controllers
{
    public class HomeController : ErrorCatchingControllerBase
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
            if ((User != null) && User.Identity.IsAuthenticated)
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
