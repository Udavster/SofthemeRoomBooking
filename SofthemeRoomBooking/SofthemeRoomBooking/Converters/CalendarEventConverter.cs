using System.Collections.Generic;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Converters
{
    public class CalendarEventConverter
    {
        public static List<CalendarEventModel>[] GetCalendarEventModels(RoomModel[] rooms, EventModel[] calendarEvent)
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
                    h = calendarEvent[i].StartTime.Hour,
                    m = calendarEvent[i].StartTime.Minute
                };

                var eventFinish = new TimeCalendar()
                {
                    h = calendarEvent[i].FinishTime.Hour,
                    m = calendarEvent[i].FinishTime.Minute
                };

                calendarEventModels[roomNum].Add(new CalendarEventModel()
                {
                    Id = calendarEvent[i].Id,
                    Title = (calendarEvent[i].Publicity) ? calendarEvent[i].Title : "",
                    Description = (calendarEvent[i].Publicity) ? calendarEvent[i].Description : "",
                    Start = eventStart,
                    Finish = eventFinish,
                    Publicity = calendarEvent[i].Publicity
                });
            }

            return calendarEventModels;
        }

    }
}
