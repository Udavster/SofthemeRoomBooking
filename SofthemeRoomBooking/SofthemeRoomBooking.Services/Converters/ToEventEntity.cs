using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToEventEntity
    {
        public static Events ToEventsEntity(this EventModel model, string userId)
        {
            return new Events
            {
                Title = model.Title,
                Description = model.Description,
                Id_room = model.IdRoom,
                Nickname = model.Nickname,
                Publicity = model.Publicity,
                Id_user = userId,
                Start = model.StartDateTime,
                Finish = model.EndDateTime,
            };
        }
    }

}
