using System;
using System.Collections.Generic;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IEventService
    {
        void AddEvent(NewEventModel model, string userId);
        EventUserModel EventInfo(int id);
        List<List<EventRoomModel>> GetEventsByWeek(DateTime date, int id);
        altEventModel[] GetEventsByDate(DateTime day);
    }
}
