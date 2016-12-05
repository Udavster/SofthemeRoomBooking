using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Converters;
using SofthemeRoomBooking.Services.Models;


namespace SofthemeRoomBooking.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private SofhemeRoomBookingContext _context;
        public RoomService(SofhemeRoomBookingContext context)
        {
            _context = context;
        }
        public string GetEventsByWeek(string date, int id)
        {
            DateTime currentDate = DateTime.ParseExact(date,
                      "yyyy-MM-dd",
                      CultureInfo.InvariantCulture);

            List<List<EventModel>> events = new List<List<EventModel>>();

            for (int i = 0; i < 8; i++)
            {
                List<EventModel> day = new List<EventModel>();
                var eventsList =
                    _context.Events.Where(
                        ev =>
                            ev.Start.Day == currentDate.Day && ev.Start.Month == currentDate.Month &&
                            ev.Id_room == id);
                foreach (var item in eventsList)
                {
                    day.Add(item.ToEvent());
                }
                events.Add(day);
                if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    currentDate = currentDate.AddDays(2);
                }
                else
                {
                    currentDate = currentDate.AddDays(1);
                }
            }

            var result = JsonConvert.SerializeObject(events);
            return result;
        }
    }
}