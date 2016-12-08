using System;
using System.Collections.Generic;
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
        public List<List<EventModel>> GetEventsByWeek(DateTime date, int id)
        {
            //DateTime currentDate = DateTime.ParseExact(date,
            //          "yyyy-MM-dd",
            //          CultureInfo.InvariantCulture);

            List<List<EventModel>> events = new List<List<EventModel>>();

            for (int i = 0; i < 8; i++)
            {
                List<EventModel> day = new List<EventModel>();
                var eventsList =
                    _context.Events.Where(
                        ev =>
                            ev.Start.Day == date.Day && ev.Start.Month == date.Month &&
                            ev.Id_room == id);
                foreach (var item in eventsList)
                {
                    day.Add(item.ToEvent());
                }
                events.Add(day);
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    date = date.AddDays(2);
                }
                else
                {
                    date = date.AddDays(1);
                }
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