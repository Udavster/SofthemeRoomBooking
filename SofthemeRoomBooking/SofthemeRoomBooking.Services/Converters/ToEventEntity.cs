using System;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToEventEntity
    {
        public static Events ToEventsEntity(this NewEventModel model, DateTime startTime, DateTime endTime, string userId)
        {
            return new Events()
            {
                Start = startTime,
                Finish = endTime,
                Title = model.Title,
                Description = model.Description,
                Id_room = model.IdRoom,
                Nickname = model.Nickname,
                Publicity = model.Publicity,
                Id_user = userId
            };
        }
    }

}
