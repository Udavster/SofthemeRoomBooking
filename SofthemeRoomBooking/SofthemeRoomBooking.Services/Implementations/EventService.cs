using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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

        public int GetEventCountByUser(string userId)
        {
            DateTime now = DateTime.Now;

            int count = 0;

            try
            {
                count =
                    _context.Events.Where(ev => (ev.Id_user == userId) && (ev.Finish < now))
                        .GroupBy(ev => ev.Id_user)
                        .Select(group => group.Count()).First();
            }
            catch (Exception)
            {
                return -1;
            }

            return count;
        }

        public IEnumerable<EventsForUserCount> GetEventCountEnumerable()
        {
            DateTime now = DateTime.Now;

            try
            {
                return _context.Events.Where(ev => (ev.Finish < now))
                        .GroupBy(ev => ev.Id_user)
                        .Select(group => new EventsForUserCount() {UserId = group.Key, EventCount = group.Count()});
            }
            catch (Exception)
            {
                return null;
            }
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

        public altEventModel[] GetEventsByDate(DateTime day)
        {
            var nextDay = day.AddDays(1);

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
            
            return query.ToArray();
        }

    }
}
