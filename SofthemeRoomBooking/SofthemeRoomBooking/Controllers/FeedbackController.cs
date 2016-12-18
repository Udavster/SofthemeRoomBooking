using System;
using System.Linq;
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
        private INotificationService _notificationService;
        private IProfileService _profileService;

        public FeedbackController(IFeedbackService feedbackService, INotificationService notificationService, IProfileService profileService)
        {
            _feedbackService = feedbackService;
            _notificationService = notificationService;
            _profileService = profileService;
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

                var admins = _profileService.GetAllAdminsEmails().ToList();

                _notificationService.FeedbackNotification(admins,feedback);
                return RedirectToAction("Index");
            }

            return View("Index", model);
            
        }
    }
}