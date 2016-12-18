using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class FeedbackController : ErrorCatchingControllerBase
    {
        private IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(FeedbackViewModel model)
        {
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext("SofthemeRoomBooking")));
            //var user= manager.FindByIdAsync(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                var feedback = new FeedbackModel()
                {
                    Email = model.Email,
                    Message = model.Message,
                    Name = model.Name,
                    Surname = model.Surname,
                    Created = DateTime.Now
                };
                //Save
                _feedbackService.Save(feedback);
                return RedirectToAction("Index");
            }

            return View("Index", model);
            
        }
    }
}