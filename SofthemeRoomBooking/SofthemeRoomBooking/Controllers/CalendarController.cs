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
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class CalendarController : Controller
    {
        private ICalendarService _calendarService;
        private IRoomService _roomService;
        private IEventService _eventService;

        public CalendarController(ICalendarService calendarService, IRoomService roomService, IEventService eventService)
        {
            _calendarService = calendarService;
            _roomService = roomService;
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult Index(string date)
        {
            DateTime day = DateTime.ParseExact(date,
                     "yyyyMMdd",
                     CultureInfo.InvariantCulture);
            
            var calendarEvent = _eventService.GetEventsByDate(date);
            var rooms = _roomService.GetUnlockedRoomsByDate(day);

            List<CalendarEventModel>[] arr = new List<CalendarEventModel>[rooms.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new List<CalendarEventModel>();
            }

            int roomNum = 0;

            for (int i = 0; i < calendarEvent.Length; i++)
            {
                if (rooms[roomNum].Id != calendarEvent[i].IdRoom)
                {
                    int j = 0;
                    for (j = 0; j < rooms.Length; j++)
                    {
                        if (rooms[j].Id == calendarEvent[i].IdRoom)
                        {
                            roomNum = j;
                            break;
                        }
                    }

                    if (j == rooms.Length)
                    {
                        continue;
                    }
                }
                
                var Start = new TimeCalendar()
                {
                    h = calendarEvent[i].Start.Hour,
                    m = calendarEvent[i].Start.Minute
                };

                TimeCalendar Finish = new TimeCalendar();
                if (calendarEvent[i].Finish.HasValue)
                {
                    Finish.h = calendarEvent[i].Finish.Value.Hour;
                    Finish.m = calendarEvent[i].Finish.Value.Minute;
                }

                arr[roomNum].Add(new CalendarEventModel()
                {
                    Id = calendarEvent[i].Id,
                    Title = calendarEvent[i].Title,
                    Description = calendarEvent[i].Description,
                    Start =  Start,
                    Finish = Finish,
                    Publicity = calendarEvent[i].Publicity
                });
            }
            //var b = new Tuple<List<CalendarEventModel>[], RoomModel[]>(arr,rooms);
            System.Collections.Hashtable ab = new Hashtable();
            ab.Add("Rooms", rooms);
            ab.Add("Events", arr);
            string json = JsonConvert.SerializeObject(ab, Formatting.Indented);

            return Content(json, "application/json");
        }
    }
}