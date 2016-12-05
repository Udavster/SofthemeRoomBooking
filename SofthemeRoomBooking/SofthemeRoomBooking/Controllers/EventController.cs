using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class EventController : Controller
    {
        private IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        // GET: Event
        public ActionResult Index()
        {
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