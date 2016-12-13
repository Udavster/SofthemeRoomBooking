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
                Day = model.StartTime.Day,
                Month = model.StartTime.Month,
                Year = model.StartTime.Year,
                StartHour = model.StartTime.Hour,
                StartMinutes = model.StartTime.Minute,
                EndHour = model.FinishTime.Hour,
                EndMinutes = model.FinishTime.Minute
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
                StartTime = new DateTime(model.Year, model.Month, model.Day, model.StartHour, model.StartMinutes, 0),
                FinishTime = new DateTime(model.Year, model.Month, model.Day, model.EndHour, model.EndMinutes, 0)
            };
        }
    }
}