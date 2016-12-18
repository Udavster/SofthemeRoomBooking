using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Converters;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly SofhemeRoomBookingContext _context;
        private readonly INotificationService _notificationService;

        public EventService(SofhemeRoomBookingContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public void CreateEvent(EventModel model, string userId)
        {
            var @event = model.ToEventEntity(userId);

            _context.Events.Add(@event);
            _context.SaveChanges();
        }

        public bool UpdateEvent(EventModel model)
        {
            var @event = _context.Events.FirstOrDefault(ev => ev.Id == model.Id);

            if (@event != null)
            {
                @event.Title = model.Title;
                @event.Description = model.Description;
                @event.Nickname = model.Nickname;
                @event.Publicity = model.Publicity;
                @event.AllowRegistration = model.AllowRegistration;
                @event.Id_room = model.IdRoom;
                @event.Start = model.StartTime;
                @event.Finish = model.FinishTime;

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CancelEvent(int eventId)
        {
            var @event = _context.Events.FirstOrDefault(ev => ev.Id == eventId);

            if (@event != null)
            {
                @event.Cancelled = true;

                _context.SaveChanges();

                var usersEmails = _context.EventsUsers.Where(ev => ev.IdEvent == eventId).Select(x => x.Email).ToList();
                var eventInfo = _context.Events.FirstOrDefault(ev => ev.Id == eventId);
                _notificationService.CancelEventNotification(usersEmails, eventInfo);
                return true;
            }

            return false;
        }

        public EventModel GetEventById(int eventId)
        {
            var @event = _context.Events.FirstOrDefault(ev => ev.Id == eventId);

            return @event.ToEventModel();
        }

        public EventDetailsModel GetEventDetailsById(int eventId)
        {
            var @event = _context.Events.FirstOrDefault(ev => ev.Id == eventId);

            if (@event != null)
            {
                var participantsQuantity = _context.EventsUsers.Count(x => x.IdEvent == @event.Id);

                return @event.ToEventDetailsModel(participantsQuantity);
            }

            return null;

            //var model = new EventDetailsModel();

            ////var eventInfo = _context.Events.Where(ev => ev.Id == id).Select(ev => new
            ////{
            ////    Event = ev,
            ////    ParticipantsQuantity = _context.EventsUsers.Count(x => x.IdEvent == ev.Id)
            ////}).FirstOrDefault();

            //if (eventInfo != null)
            //{
            //    model = eventInfo.Event.ToEventUserModelConverter(eventInfo.ParticipantsQuantity, dayOfWeek, monthArray);
            //}
            //return model;
        }

        public EventModel[] GetEventsByDate(DateTime day)
        {
            var nextDay = day.AddDays(1);

            var events = from Event in _context.Events
                         where Event.Start >= day && Event.Finish < nextDay && !Event.Cancelled
                         orderby Event.Id_room, Event.Start
                         select new EventModel
                         {
                            Id = Event.Id,                         
                            Title = Event.Title,
                            Description = Event.Description,
                            Publicity = Event.Publicity,
                            Nickname = Event.Nickname,
                            IdRoom = Event.Id_room,
                            StartTime = Event.Start,
                             FinishTime = Event.Finish
                         };

            return events.ToArray();
        }

        public List<List<EventWeekModel>> GetEventsByWeek(DateTime date, int id)
        {

            List<List<EventWeekModel>> events = new List<List<EventWeekModel>>();
            var endTime = date.Date.Add(TimeSpan.FromDays(10));
            var event1 =
                _context.Events.Where(
                    ev => DbFunctions.TruncateTime(ev.Start) >= DbFunctions.TruncateTime(date) &&
                    DbFunctions.TruncateTime(ev.Start) <= DbFunctions.TruncateTime(endTime)
                    && ev.Id_room == id && ev.Cancelled == false);
            var currentDate = date;
            for (int i = 0; i < 8; i++)
            {
                List<EventWeekModel> day = new List<EventWeekModel>();
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
    }
}
