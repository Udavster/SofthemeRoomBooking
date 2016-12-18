using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.EventViewModel;
using SofthemeRoomBooking.Services.Contracts;



namespace SofthemeRoomBooking.Controllers
{
    [Authorize]
    public class EventController : ErrorCatchingControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IRoomService _roomService;
        private readonly IProfileService _profileService;

        public EventController(IEventService eventService,IRoomService roomService, ApplicationUserManager userManager, IProfileService profileService)
        {
            _eventService = eventService;
            _roomService = roomService;
            _profileService = profileService;
        }
        
        [AllowAnonymous]
        public ActionResult Index(int eventId = 5)
        {
            var model = _eventService.GetEventIndexModelById(eventId);

            if (model != null)
            {
                var organizator = _profileService.GetUserById(model.IdUser);
                var modelView = model.ToEventIndexViewModel(organizator);

                return View(modelView);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CreateEvent()
        {
            var rooms = _roomService.GetUnlockedRoomsByDate(DateTime.Today);

            var modelView = new EventViewModel();
            modelView.SetUnlockedRooms(rooms);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(EventViewModel modelView)
        {
            var model = modelView.ToEventModel();

            if (ModelState.IsValid)
            {
                if (_roomService.IsBusyRoom(model.IdRoom, model.StartTime, model.FinishTime))
                {
                    return Json(new { success = false, errorMessage = "Эта аудитория занята на выбраное время. Выберите, пожалуйста, другое." });
                }

                _eventService.UpdateEvent(model);

                return Json(new { success = true, redirectTo = Url.Action("Index", "Home") });
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return PartialView("_EditEventPartial", modelView);
        }

        [HttpGet]
        public ActionResult EditEventPartial(int eventId = -1)
        {
            var rooms = _roomService.GetUnlockedRoomsByDate(DateTime.Today);

            EventViewModel modelView;
            if (eventId == -1)
            {
                modelView = new EventViewModel();
                modelView.SetUnlockedRooms(rooms);

                return PartialView("_EditEventPartial", modelView);
            }

            var model = _eventService.GetEventById(eventId);

            if (model != null)
            {
                modelView = model.ToEventViewModel(rooms);

                return PartialView("_EditEventPartial", modelView);
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEventPartial(EventViewModel modelView)
        {
            var model = modelView.ToEventModel();

            if (ModelState.IsValid)
            {
                if (_roomService.IsBusyRoom(model.IdRoom, model.StartTime, model.FinishTime))
                {
                    return Json(new { success = false, errorMessage = "Эта аудитория занята на выбраное время. Выберите, пожалуйста, другое." });
                }

                _eventService.UpdateEvent(model);

                return Json(new { success = true, redirectTo = HttpContext.Request.RawUrl.Replace("Event/EventEditPartial","") });
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
                return PartialView("_EventDetailsPrivatePartial", viewModel);
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
        [ActionName("CancelEvent")]
        public ActionResult CancelEventConfirm(int id)
        {
            _eventService.CancelEvent(id);
            return Content(@"Home/Index");
        }

        public ActionResult EventParticipants(int eventId)
        {
            var model = _eventService.GetParticipantsByEventId(eventId);

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult AddParticipant(int eventId)
        {
            var modelView = new EventParticipantViewModel { IdEvent = eventId };

            return PartialView(modelView);
        }

        [HttpPost]
        public ActionResult AddParticipant(EventParticipantViewModel modelView)
        {
            if (ModelState.IsValid)
            {
                var model = modelView.ToEventParticipantModel();
                _eventService.CreateParticipant(model);
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", modelView.IdEvent);
        }

        [HttpGet]
        public ActionResult DeleteParticipant(int participantId)
        {
            var participant = _eventService.GetParticipantById(participantId);

            if (participant != null)
            {
                var model = new ConfirmationViewModel
                {
                    Question = "Вы уверены, что хотите исключить этого участника из события?",
                    Message = participant.Email,
                    Action = "Delete",
                    Controller = "Profile",
                    DataId = participant.Id.ToString()
                };

                return PartialView("_PopupConfirmationPartial", model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ActionName("DeleteParticipant")]
        public ActionResult DeleteParticipantConfirm(int id)
        {
            var result = _eventService.DeleteParticipant(id);

            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }
    }
}