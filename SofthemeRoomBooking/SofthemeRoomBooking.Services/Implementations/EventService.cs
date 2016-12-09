using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Implementations
{
    public class EventService : IEventService
    {
        private SofhemeRoomBookingContext _context;
        public EventService(SofhemeRoomBookingContext context)
        {
            _context = context;
        }


        public void AddEvent(EventModel model, string userId)
        {
            Events newEvent = new Events()
            {
                Start = DateTime.Parse(model.Start),
                Finish = DateTime.Parse(model.Finish),
                Title = model.Title,
                Description = model.Description,
                Id_room = model.Id_room,
                Nickname = model.Nickname,
                Publicity = model.Publicity,
                Id_user = userId

            };

            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }
        public altEventModel[] GetEventsByDate(string dateString)
        {
            DateTime day = DateTime.ParseExact(dateString,
                      "yyyyMMdd",
                      CultureInfo.InvariantCulture);
            var nextDay = day.AddDays(1);

            //var result = _context.Events.Where(
            //    ev => !ev.Cancelled && (ev.Start >= day) && (ev.Start < nextDay)
            //).Select(ev => new altEventModel() {Id = ev.Id, IdRoom = ev.Id_room, IdUser = ev.Id_user, Start = ev.Start, });

            //            SELECT R.Id AS RoomId, R.Name, E.Id AS EventId FROM Rooms AS R
            //LEFT JOIN Events AS E ON E.Id_room = R.Id
            //WHERE E.Id IS NULL OR E.Cancelled = 0
            //ORDER BY R.Id

            //var query = from p in Programs
            //            join pl in ProgramLocations
            //                on p.ProgramID equals pl.ProgramID into pp
            //            from pl in pp.DefaultIfEmpty()
            //            where pl == null
            //            select p;

            //var query = from Room in _context.Rooms
            //    join Event in _context.Events
            //    on Room.Id equals Event.Id_room into wh
            //    from Event in wh.DefaultIfEmpty()
            //    where Event == null || !Event.Cancelled
            //    select Room;

             var query = from Event in _context.Events
                where Event.Start >= day && Event.Finish < nextDay && !Event.Cancelled
                orderby Event.Id_room, Event.Start
                select new altEventModel()
                {
                    Id = Event.Id,
                    IdRoom = Event.Id_room,
                    Title = Event.Title,
                    Description  = Event.Description,
                    Start = Event.Start,
                    Finish = Event.Finish,
                    Publicity = Event.Publicity
                };
           //var a = query.ToList();

            return query.ToArray();
        }
    }
}
