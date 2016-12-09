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
        public EventController(IEventService eventService,IRoomService roomService)
        {
            _eventService = eventService;
            _roomService = roomService;
        }
        // GET: Event
        public ActionResult Index()
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
                Value = r.IdRoom.ToString(),
                Selected = "select" == r.IdRoom.ToString()
            }).ToList();

            NewEventViewModel eventModel = new NewEventViewModel()
            {
                Rooms = dropDownList
            };

            return PartialView(eventModel);
        }
    }
}