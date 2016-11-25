using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Save(Feedback model)
        {
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