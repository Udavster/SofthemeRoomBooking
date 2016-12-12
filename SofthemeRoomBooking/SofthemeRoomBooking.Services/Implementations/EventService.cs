using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Converters;
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
        public List<List<EventRoomModel>> GetEventsByWeek(DateTime date, int id)
        {

            List<List<EventRoomModel>> events = new List<List<EventRoomModel>>();
            var endTime = date.Date.Add(TimeSpan.FromDays(10));
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
        public void AddEvent(NewEventModel model, string userId)
        {
            DateTime startTime = DateTime.Parse(DateTime.Now.Year+"-"+model.Month+"-"+model.Day+" "+model.StartHour+":"+model.StartMinute);
            DateTime endTime = DateTime.Parse(DateTime.Now.Year+"-"+model.Month +"-"+model.Day+" "+model.EndHour+":"+model.EndMinute);

            Events newEvent = model.ToEventsEntity(startTime, endTime, userId);

            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }

        public EventUserModel EventInfo(int id)
        {
            string[] monthArray =
            {
                "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября",
                "Октября", "Ноября", "Декабря"
            };
            string[] dayOfWeek =
            {
                "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Cб" 
            };
            var model = new EventUserModel();
            var eventInfo = _context.Events.Where(ev => ev.Id == id).Select(ev => new
            {
                Event = ev,
                ParticipantsQuantity = _context.EventsUsers.Count(x => x.IdEvent == ev.Id)
            }).FirstOrDefault();

            if (eventInfo != null)
            {
                 model= eventInfo.Event.ToEventUserModelConverter(eventInfo.ParticipantsQuantity,dayOfWeek,monthArray);
            }
            return model;
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
