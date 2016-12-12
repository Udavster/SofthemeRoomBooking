using System;
using System.Collections.Generic;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class EventConverter
    {
        public static EventViewModel ToEventViewModel(this NewEventModel model, List<RoomModel> rooms)
        {
            return new EventViewModel(rooms)
            {
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

        public static NewEventModel ToNewEventModel(this EventViewModel model)
        {
            return new NewEventModel
            {
                Title = model.Title,
                Description = model.Description,
                IdRoom = model.IdRoom,
                Nickname = model.Nickname,
                Publicity = !model.Private,
                StartDateTime = DateTime.Parse($"{model.Year}-{model.Month}-{model.Day} {model.StartHour}:{model.StartMinutes}"),
                EndDateTime = DateTime.Parse($"{model.Year}-{model.Month}-{model.Day} {model.EndHour}:{model.EndMinutes}")
            };
        }
    }
}