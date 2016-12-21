using System;
using System.Collections;
using System.Globalization;
using System.Web.Mvc;
using SofthemeRoomBooking.Services.Contracts;
using Newtonsoft.Json;
using SofthemeRoomBooking.Converters;

namespace SofthemeRoomBooking.Controllers
{
    public class CalendarController : ErrorCatchingControllerBase
    {
        private IRoomService _roomService;
        private IEventService _eventService;

        public CalendarController(IRoomService roomService, IEventService eventService)
        {
            _roomService = roomService;
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult Index(string date, string profileId)
        {
            DateTime day;
            try
            {
                day = DateTime.ParseExact(date, "yyyyMMdd",
                                          CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return Content("{\n\"error\": \n\"true\"\n}", "application/json");
            }


            var calendarEvent = (profileId==null)?
                                _eventService.GetEventsByDate(day):
                                _eventService.GetEventsByDateAndProfile(day, profileId);

            var rooms = _roomService.GetUnlockedRoomsByDate(day);

            var calendarEventListArray = CalendarEventConverter.GetCalendarEventModels(rooms, calendarEvent);

            System.Collections.Hashtable ab = new Hashtable {
                                                                { "Rooms", rooms},
                                                                { "Events", calendarEventListArray},
                                                                { "Authenticated", (User.Identity.IsAuthenticated)}
            };
            string json = JsonConvert.SerializeObject(ab, Formatting.Indented);

            return Content(json, "application/json");
        }


    }
}