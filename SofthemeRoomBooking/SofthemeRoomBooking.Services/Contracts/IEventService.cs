using System;
using System.Collections.Generic;
using System.Linq;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IEventService
    {
        void CreateEvent(EventModel model, string userId);

        bool UpdateEvent(EventModel model);

        bool CancelEvent(int eventId);

        EventModel GetEventById(int eventId);

        EventDetailsModel GetEventDetailsById(int eventId);

        EventModel[] GetEventsByDate(DateTime day);

        List<List<EventWeekModel>> GetEventsByWeek(DateTime date, int id);
    }
}
