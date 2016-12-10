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

            var calendarEventListArray = GetCalendarEventModels(rooms, calendarEvent);

            System.Collections.Hashtable ab = new Hashtable {{"Rooms", rooms}, {"Events", calendarEventListArray}};
            string json = JsonConvert.SerializeObject(ab, Formatting.Indented);

            return Content(json, "application/json");
        }

        private static List<CalendarEventModel>[] GetCalendarEventModels(RoomModel[] rooms, altEventModel[] calendarEvent)
        {
            var calendarEventModels = new List<CalendarEventModel>[rooms.Length];

            for (int i = 0; i < calendarEventModels.Length; i++)
            {
                calendarEventModels[i] = new List<CalendarEventModel>();
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

                var eventStart = new TimeCalendar()
                {
                    h = calendarEvent[i].Start.Hour,
                    m = calendarEvent[i].Start.Minute
                };

                var eventFinish = new TimeCalendar()
                {
                    h = calendarEvent[i].Finish.Hour,
                    m = calendarEvent[i].Finish.Minute
                };

                calendarEventModels[roomNum].Add(new CalendarEventModel()
                {
                    Id = calendarEvent[i].Id,
                    Title = calendarEvent[i].Title,
                    Description = calendarEvent[i].Description,
                    Start = eventStart,
                    Finish = eventFinish,
                    Publicity = calendarEvent[i].Publicity
                });
            }

            return calendarEventModels;
        }

    }
}