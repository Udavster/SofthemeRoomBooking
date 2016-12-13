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
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IRoomService _roomService;
        private readonly ApplicationUserManager _userManager;

        public EventController(IEventService eventService,IRoomService roomService, ApplicationUserManager userManager)
        {
            _eventService = eventService;
            _roomService = roomService;
            _userManager = userManager;
        }

        // GET: Event
        public ActionResult Index(string date)
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateEvent()
        {
            var rooms = _roomService.GetUnlockedRoomsByDate(DateTime.Today);
            var modelView = new EventViewModel(rooms);

            return PartialView("_CreateEventPartial", modelView);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(EventViewModel viewModel)
        {
            var model = viewModel.ToEventModel();
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                //if the room is not occupied at this time
                //save
                //else
                //return error
                _eventService.CreateEvent(model, userId);
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditEvent(int eventId)
        {
            var model = _eventService.GetEventById(eventId);

            if (model != null)
            {
                var rooms = _roomService.GetUnlockedRoomsByDate(model.StartDateTime);
                var modelView = model.ToEventViewModel(rooms);

                return PartialView("_EditEventPartial", modelView);
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
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
    }
}