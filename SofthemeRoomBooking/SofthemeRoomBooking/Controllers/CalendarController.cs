using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Contracts;
using Newtonsoft.Json;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class CalendarController : Controller
    {
        private IRoomService _roomService;
        private IEventService _eventService;

        public CalendarController(IRoomService roomService, IEventService eventService)
        {
            _roomService = roomService;
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult Index(string date)
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

            var calendarEvent = _eventService.GetEventsByDate(day);
            var rooms = _roomService.GetUnlockedRoomsByDate(day);

            var calendarEventListArray = CalendarEventConverter.GetCalendarEventModels(rooms, calendarEvent);

            System.Collections.Hashtable ab = new Hashtable {{"Rooms", rooms}, {"Events", calendarEventListArray}};
            string json = JsonConvert.SerializeObject(ab, Formatting.Indented);

            return Content(json, "application/json");
        }


    }
}