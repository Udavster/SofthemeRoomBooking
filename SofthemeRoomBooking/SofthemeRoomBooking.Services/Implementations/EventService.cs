﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Converters;
using SofthemeRoomBooking.Services.Models.EventModel;

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

        public int CreateEvent(EventModel model, string userId)
        {
            var @event = model.ToEventEntity(userId);

            _context.Events.Add(@event);
            _context.SaveChanges();

            return @event.Id;
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

        public bool CancelEvent(int eventId, string creatorEmail)
        {
            var @event = _context.Events.FirstOrDefault(ev => ev.Id == eventId);

            if (@event != null)
            {
                @event.Cancelled = true;

                _context.SaveChanges();

                var usersEmails = _context.EventsUsers.Where(ev => ev.IdEvent == eventId).Select(x => x.Email).ToList();
                usersEmails.Add(creatorEmail);

                var eventInfo = _context.Events.FirstOrDefault(ev => ev.Id == eventId);
                var roomInfo = _context.Rooms.FirstOrDefault(x => x.Id == eventInfo.Id_room);
                if (roomInfo != null)
                {
                    var roomName = roomInfo.Name;
                    _notificationService.CancelEventNotification(usersEmails, eventInfo, roomName);
                }
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

        public EventIndexModel GetEventIndexModelById(int eventId)
        {
            var @event = _context.Events.FirstOrDefault(ev => ev.Id == eventId);

            if (@event != null)
            {
                var participantsQuantity = GetParticipantCountByEventId(eventId);

                return @event.ToEventIndexModel(participantsQuantity);
            }

            return null;
        }

        public void CreateParticipant(EventParticipantModel model, string creatorEmail)
        {
            var participant = model.ToEventParticipantEntity();
            
            _context.EventsUsers.Add(participant);
            _context.SaveChanges();

            var @event = _context.Events.Where(ev => ev.Id == model.IdEvent).FirstOrDefault();
            _notificationService.EventUserAddedNotification(creatorEmail, participant.Email, @event);
        }

        public bool DeleteParticipant(int participantId)
        {
            //?
            var participant = _context.EventsUsers.FirstOrDefault(eu => eu.Id == participantId);

            if (participant != null)
            {
                _context.EventsUsers.Remove(participant);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool ContainsParticipantInEvent(EventParticipantModel model)
        {
            return _context.EventsUsers.Any(eu => eu.IdEvent == model.IdEvent && eu.Email == model.Email);
        }

        public EventParticipantModel GetParticipantById(int participantId)
        {
            var participant = _context.EventsUsers.FirstOrDefault(eu => eu.Id == participantId);

            if (participant != null)
            {
                var model = participant.ToEventParticipantModel();

                return model;
            }

            return null;
        }

        public IQueryable<EventParticipantModel> GetParticipantsByEventId(int eventId)
        {
            var participants = _context.EventsUsers.Where(eu => eu.IdEvent == eventId)
                                                   .Select(eu => new EventParticipantModel
                                                   {
                                                       Id = eu.Id,
                                                       Email = eu.Email
                                                   });

            return participants;
        }

        public int GetParticipantCountByEventId(int eventId)
        {
            return _context.EventsUsers.Count(eu => eu.IdEvent == eventId);
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
        public EventModel[] GetEventsByDateAndProfile(DateTime day, string profileId)
        {
            if (profileId == null)
            {
                throw new ArgumentNullException();
            }

            var nextDay = day.AddDays(1);

            var events = from Event in _context.Events
                         where Event.Start >= day && Event.Finish < nextDay && !Event.Cancelled && Event.Id_user == profileId
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
                    day.Add(!eventItem.Publicity ? eventItem.ToPrivateEvent() : eventItem.ToEvent());
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
                    _context.Events.Where(ev => (ev.Id_user == userId) && (ev.Start > now))
                        .GroupBy(ev => ev.Id_user)
                        .Select(group => group.Count()).FirstOrDefault();
            } catch (Exception ex)

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
                return _context.Events.Where(ev => (ev.Start > now))
                        .GroupBy(ev => ev.Id_user)
                        .Select(group => new EventsForUserCount() { UserId = group.Key, EventCount = group.Count() });
            } catch (Exception)
            {
                return null;
            }
        }

        public List<EventCreatorModel> GetEventsByRoom(int roomId)
        {
            var now = DateTime.Now;
            List<EventCreatorModel> model = new List<EventCreatorModel>();
            var events = _context.Events.Where(ev => (ev.Id_room == roomId) && (!ev.Cancelled) && (ev.Start >= now));

            foreach (var item in events)
            {
                model.Add(item.ToEventCreator());
            }
            return model;
        }
    }
}
