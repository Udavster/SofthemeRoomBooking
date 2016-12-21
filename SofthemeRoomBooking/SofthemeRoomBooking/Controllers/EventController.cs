using System;
using System.Linq;
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
        public ActionResult Index(int id)
        {
            var model = _eventService.GetEventIndexModelById(id);

            if (model != null)
            {
                var organizator = _profileService.GetUserById(model.IdUser);
                var modelView = model.ToEventIndexViewModel(organizator);

                var currUserId = User.Identity.GetUserId();
                modelView.IsAdminOrOrganizator = _profileService.IsAdmin(currUserId) || model.IdUser == currUserId;

                return View("Index", modelView);
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
                    return Json(new { success = false, errorMessage = "Эта аудитория занята на выбраное время. Выберите, пожалуйста, другое." });
                }

                var eventId = _eventService.CreateEvent(model, userId);

                return Json(new { success = true, redirectTo = "/Event/Index/" + eventId });
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(EventIndexViewModel modelView)
        {
            if (ModelState.IsValid)
            {
                var model = modelView.ToEventModel();

                if (_roomService.IsBusyRoom(model.IdRoom, model.StartTime, model.FinishTime, model.Id))
                {
                    return Json(new { success = false, errorMessage = "Эта аудитория занята на выбраное время. Выберите, пожалуйста, другое." });
                }

                _eventService.UpdateEvent(model);

                return Json(new { success = true, redirectTo = Url.Action("Index", "Event", new { id = model.Id }) });
            }

            var errors = string.Join(". ", ModelState.Values.Where(e => e.Errors.Count > 0)
                                                                        .SelectMany(e => e.Errors)
                                                                        .Select(e => e.ErrorMessage)
                                                                        .ToArray());

            return Json(new { success = false, errorMessage = errors });
        }

        [HttpGet]
        public ActionResult EditEventPartial(int? eventId)
        {
            var rooms = _roomService.GetUnlockedRoomsByDate(DateTime.Today);

            EventViewModel modelView;
            if (eventId == null)
            {
                modelView = new EventViewModel { Id = null};
                modelView.SetUnlockedRooms(rooms);

                return PartialView("_EditEventPartial", modelView);
            }

            var model = _eventService.GetEventById(eventId.Value);

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
                if (_roomService.IsBusyRoom(model.IdRoom, model.StartTime, model.FinishTime, model.Id))
                {
                    return Json(new { success = false, errorMessage = "Эта аудитория занята на выбраное время. Выберите, пожалуйста, другое." });
                }

                if (modelView.Id == null)
                {
                    var userId = User.Identity.GetUserId();

                    _eventService.CreateEvent(model, userId);
                }
                else
                {
                    _eventService.UpdateEvent(model);
                }
                
                return Json(new { success = true, redirectTo = HttpContext.Request.QueryString });
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

            return PartialView("_EventDetailPublicPartial",viewModel);
        }

        [HttpGet]
        public ActionResult CancelEvent(int id)
        {
            var model = new ConfirmationViewModel
            {
                Question = "Вы уверены, что хотите отменить событие?",
                Message = "",
                Action = "CancelEvent",
                Controller = "Event",
                DataId = id.ToString()
            };

            return PartialView("_PopupConfirmationPartial", model);
        }

        [HttpPost]
        [ActionName("CancelEvent")]
        public ActionResult CancelEventConfirm(int id)
        {
            var currentEvent = _eventService.GetEventIndexModelById(id);
            var creatorEmail = _profileService.GetUserById(currentEvent.IdUser).Email;
            _eventService.CancelEvent(id,creatorEmail);
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EventParticipants(int id)
        {
            var model = _eventService.GetParticipantsByEventId(id);

            var modelView = model.Select(x => new EventParticipantViewModel
            {
                Id = x.Id,
                Email = x.Email
            }).ToList();

            return PartialView(modelView);
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

                if (_eventService.ContainsParticipantInEvent(model))
                {
                    return Json(new { success = false, errorMessage = "Данный email уже зарегистрирован на это событие." });
                }

                var creatorId = _eventService.GetEventIndexModelById(modelView.IdEvent).IdUser;
                var creatorEmail = _profileService.GetUserById(creatorId).Email;

                _eventService.CreateParticipant(model, creatorEmail);
		return Json(new { success = true, redirectTo = HttpContext.Request.QueryString });
            }
            var errors = string.Join(". ", ModelState.Values.Where(e => e.Errors.Count > 0)
                                                                        .SelectMany(e => e.Errors)
                                                                        .Select(e => e.ErrorMessage)
                                                                        .ToArray());

            return Json(new { success = false, errorMessage = errors });
        }

        [HttpGet]
        public ActionResult DeleteParticipant(int id)
        {
            var participant = _eventService.GetParticipantById(id);

            if (participant != null)
            {
                var model = new ConfirmationViewModel
                {
                    Question = "Вы уверены, что хотите исключить этого участника из события?",
                    Message = participant.Email,
                    Action = "DeleteParticipant",
                    Controller = "Event",
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