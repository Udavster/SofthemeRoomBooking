using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class EventController : Controller
    {
        private IEventService _eventService;
        private ICalendarService _calendarService;
        public EventController(IEventService eventService, ICalendarService calendarService)
        {
            _eventService = eventService;
            _calendarService = calendarService;
        }
        // GET: Event
        public ActionResult Index(string date)
        {
            //_eventService.GetEventsByDate(date);
            _calendarService.GetEventsByDate(date);
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Event(EventModel model)
        {
            var userId = User.Identity.GetUserId();
            _eventService.AddEvent(model,userId);
            return Content("","application/json");
        }
    }
}