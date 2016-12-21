using System;
using System.Collections.Generic;
using System.Linq;
using SofthemeRoomBooking.Services.Models.EventModel;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IEventService
    {
        int CreateEvent(EventModel model, string userId);

        bool UpdateEvent(EventModel model);

        bool CancelEvent(int eventId, string creatorEmail);

        EventModel GetEventById(int eventId);

        EventDetailsModel GetEventDetailsById(int eventId);

        EventIndexModel GetEventIndexModelById(int eventId);

        EventModel[] GetEventsByDate(DateTime day);

        EventModel[] GetEventsByDateAndProfile(DateTime day, string profileId);
        
        List<List<EventWeekModel>> GetEventsByWeek(DateTime date, int id);

        void CreateParticipant(EventParticipantModel model);

        bool DeleteParticipant(int participantId);

        EventParticipantModel GetParticipantById(int participantId);

        IQueryable<EventParticipantModel> GetParticipantsByEventId(int eventId);

        int GetParticipantCountByEventId(int eventId);

        List<EventCreatorModel> GetEventsByRoom(int roomId);
    }
}
