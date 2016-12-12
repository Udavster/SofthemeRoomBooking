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
        private IEventService _eventService;
        private IRoomService _roomService;
        private ApplicationUserManager _userManager;

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
            var rooms = _roomService.GetAllRooms();
            var model = new EventViewModel(rooms);

            return PartialView("_CreateEventPartial", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent(EventViewModel viewModel)
        {
            var model = viewModel.ToNewEventModel();
            var userId = User.Identity.GetUserId();
            return RedirectToAction("Index", "Home");
            _eventService.AddEvent(model, userId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditEvent(string eventId)
        {
            var rooms = _roomService.GetAllRooms();

            var model = new EventViewModel(rooms);

            return PartialView("_EditEventPartial", model);
            //logic
            //var model = _eventService.GetEventViewModelById(eventId);

            //if (model != null)
            //{
            //    return PartialView("_EditEventPartial", model);
            //}

            //return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent(EventViewModel model)
        {
            return PartialView("_EditEventPartial");
            //var result = await _eventService.Edit(model);

            //if (!result)
            //{
            //    ModelState.AddModelError("", "Что-то пошло не так");
            //    return PartialView("_EditEventPartial", model);
            //}

            //return RedirectToAction("Index", "Home");
        }
    }
}