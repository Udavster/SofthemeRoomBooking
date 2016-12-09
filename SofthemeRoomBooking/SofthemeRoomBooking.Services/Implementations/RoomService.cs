using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
        public List<List<EventRoomModel>> GetEventsByWeek(DateTime date, int id)
        {

            List<List<EventRoomModel>> events = new List<List<EventRoomModel>>();
            var endTime  = date.Date.Add(TimeSpan.FromDays(10));
            var event1 =
                _context.Events.Where(
                    ev => DbFunctions.TruncateTime(ev.Start) >= DbFunctions.TruncateTime(date) &&
                    DbFunctions.TruncateTime(ev.Start) <= DbFunctions.TruncateTime(endTime)
                    && ev.Id_room == id);
            var currentDate = date;
            for (int i = 0; i < 8; i++)
            {
                List<EventRoomModel> day = new List<EventRoomModel>();
                if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    events.Add(day);
                    currentDate = currentDate.AddDays(1);
                    continue;
                }
                if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    events.Add(day);
                   currentDate = currentDate.AddDays(2);
                    continue;
                }
                var currDay = event1.Where(ev => ev.Start.Day == currentDate.Day);
                foreach (var eventItem in currDay)
                {
                    day.Add(eventItem.ToEvent());
                }
                events.Add(day);
                currentDate = currentDate.AddDays(1);

            }
            return events;
        }

        public List<RoomModel> GetAllRooms()
        {
            List<RoomModel> roomsList = new List<RoomModel>(); 
            var rooms = _context.Rooms;
            foreach (var roomEntity in rooms)
            {
                roomsList.Add(roomEntity.ToRoom());
            }
            return roomsList;
        }

    }
}