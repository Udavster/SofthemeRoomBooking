using System;
using System.Collections.Generic;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IEventService
    {
        EventModel GetEventById(int eventId);

        void CreateEvent(EventModel model, string userId);

        void UpdateEvent(EventModel model);

        void DeleteEvent(int eventId);

        EventUserModel EventInfo(int id);

        List<List<EventRoomModel>> GetEventsByWeek(DateTime date, int id);

        altEventModel[] GetEventsByDate(DateTime day);
    }
}
