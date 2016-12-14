using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IRoomService _roomService;
        private readonly ApplicationUserManager _userManager;
        private readonly IProfileService _profileService;

        public EventController(IEventService eventService,IRoomService roomService, ApplicationUserManager userManager, IProfileService profileService)
        {
            _eventService = eventService;
            _roomService = roomService;
            _userManager = userManager;
            _profileService = profileService;
        }
        
        [AllowAnonymous]
        public ActionResult Index(string date)
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent()
        {
            var rooms = _roomService.GetUnlockedRoomsByDate(DateTime.Today);
            var modelView = new EventViewModel(rooms);

            return PartialView("_CreateEventPartial", modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(EventViewModel modelView)
        {
            var model = modelView.ToEventModel();
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                if (_roomService.IsBusyRoom(model.IdRoom, model.StartTime, model.FinishTime))
                {
                    return Json(new { errorMessage = "Эта аудитория занята на выбраное время. Выберите, пожалуйста, другое." });
                }

                _eventService.CreateEvent(model, userId);

                return Json(new { redirectTo = Url.Action("Index", "Home") });
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditEvent(int eventId)
        {
            var model = _eventService.GetEventById(eventId);

            if (model != null)
            {
                var rooms = _roomService.GetUnlockedRoomsByDate(model.StartTime);
                var modelView = model.ToEventViewModel(rooms);

                return PartialView("_EditEventPartial", modelView);
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(EventViewModel modelView)
        {
            var model = modelView.ToEventModel();

            if (ModelState.IsValid)
            {
                //if the room is not occupied at this time
                //save
                //else
                //return error
                _eventService.UpdateEvent(model);
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return PartialView("_EditEventPartial", modelView);
        }

        [AllowAnonymous]
        public ActionResult EventDetails(int id)
        {
            var model = _eventService.GetEventDetailsById(id);
            var eventCreatorName = _profileService.GetUserById(model.UserId).Name;

            var viewModel = model.ToEventDetailsViewModel(eventCreatorName);

            if (!model.Publicity)
            {
                var currentUserId = User.Identity.GetUserId();
                if ((model.UserId != currentUserId)&&(!_profileService.IsAdmin(currentUserId)))
                {
                    return new EmptyResult();
                }
            }

            return PartialView("_EventDetailEditPartial",viewModel);
        }

        [HttpGet]
        public ActionResult CancelEventView(int id)
        {
            CancelPopupViewModel model = new CancelPopupViewModel()
            {
                Id = id
            };
            return PartialView("_EventCancelationPopup",model);
        }

        [HttpPost]
        public ActionResult CancelEvent(int id)
        {
            _eventService.CancelEvent(id);
            return Content(@"Home/Index");
        }
    }
}