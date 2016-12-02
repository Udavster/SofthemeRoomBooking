using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class FeedbackController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(FeedbackViewModel model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext("SofthemeRoomBooking")));
            var user= manager.FindByIdAsync(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {

                //Save
                return RedirectToAction("Index");
            }
            else
            {
                return View("Feedback", model);
            }
        }
    }
}