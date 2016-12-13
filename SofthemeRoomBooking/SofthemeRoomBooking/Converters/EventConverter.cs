using System;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class EventConverter
    {
        public static EventViewModel ToEventViewModel(this EventModel model, RoomModel[] rooms)
        {
            return new EventViewModel(rooms)
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Private = !model.Publicity,
                IdRoom = model.IdRoom,
                Nickname = model.Nickname,
                Day = model.StartDateTime.Day,
                Month = model.StartDateTime.Month,
                Year = model.StartDateTime.Year,
                StartHour = model.StartDateTime.Hour,
                StartMinutes = model.StartDateTime.Minute,
                EndHour = model.EndDateTime.Hour,
                EndMinutes = model.EndDateTime.Minute
            };
        }

        public static EventModel ToEventModel(this EventViewModel model)
        {
            return new EventModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                IdRoom = model.IdRoom,
                Nickname = model.Nickname,
                Publicity = !model.Private,
                StartDateTime = new DateTime(model.Year, model.Month, model.Day, model.StartHour, model.StartMinutes, 0),
                EndDateTime = new DateTime(model.Year, model.Month, model.Day, model.EndHour, model.EndMinutes, 0)
            };
        }
    }
}