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

        [HttpPost]
        [Authorize]
        public ActionResult AddEvent(NewEventViewModel viewModel)
        {
            var model = viewModel.ToNewEventModel();
            var userId = User.Identity.GetUserId();
            _eventService.AddEvent(model, userId);
            return Redirect("Home/Index");
        }

        public ActionResult NewEvent()
        {
            var rooms = _roomService.GetAllRooms();

            var dropDownList = rooms.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString(),
                Selected = "select" == r.Id.ToString()
            }).ToList();

            NewEventViewModel eventModel = new NewEventViewModel()
            {
                Rooms = dropDownList
            };

            return PartialView(eventModel);
        }

        public ActionResult EventDetails(int id)
        {
            
            var model = _eventService.EventInfo(id);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == model.UserId).Name;
            ViewBag.UserName = user;
            return PartialView("_EventDetailEditPartial",model);
        }
        [HttpGet]
        [Authorize]
        public ActionResult CancelEventView(int id)
        {
            CancelPopupViewModel model = new CancelPopupViewModel()
            {
                Id = id
            };
            return PartialView("_EventCancelationPopup",model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CancelEvent(int id)
        {
            _eventService.CancelEvent(id);
            return Content(@"Home/Index");
        }
    }
}