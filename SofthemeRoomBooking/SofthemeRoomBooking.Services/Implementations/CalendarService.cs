using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Converters;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Implementations
{
    public class CalendarService:ICalendarService
    {
        private SofhemeRoomBookingContext _context;
        public CalendarService(SofhemeRoomBookingContext context)
        {
            _context = context;
        }

        public altEventModel[] GetEventsByDate(string dateString)
        {
            //DateTime day = DateTime.ParseExact(dateString,
            //          "yyyyMMdd",
            //          CultureInfo.InvariantCulture);
            //var nextDay = day.AddDays(1);

            //var query = from Room in _context.Rooms
            //    join Event in _context.Events
            //    on Room.Id equals Event.Id_room into wh
            //    from Event in wh.DefaultIfEmpty()
            //    where Event == null || !Event.Cancelled
            //    select new {Event, RoomName = Room.Name};

            //var a = query.ToList();
            //var result = a.Select(el => (el.Event == null
            //    ? new altEventModel()
            //    {
            //         RoomName = el.RoomName
            //    }
            //    : new altEventModel()
            //    {
            //        Id = el.Event.Id,
            //        Title = el.Event.Title,
            //        Description = el.Event.Description,
            //        RoomName = el.RoomName,
            //        Start = el.Event.Start,
            //        Finish = el.Event.Finish,
            //        Publicity = el.Event.Publicity
            //    })).ToArray();
            

            return null;
        }
    }
}
