using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Contracts;
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
                Publicity = model.Publicity,
                IdRoom = model.IdRoom,
                Nickname = model.Nickname,
                Day = model.Day,
                Month = model.Month,
                StartHour = model.StartHour,
                StartMinutes = model.StartMinute,
                EndHour = model.EndHour,
                EndMinutes = model.EndMinute,
            };
        }

        public static NewEventModel ToNewEventModel(this EventViewModel eventViewModel)
        {
            return new NewEventModel
            {
                Day = eventViewModel.Day,
                Month = eventViewModel.Month,
                StartHour = eventViewModel.StartHour,
                StartMinute = eventViewModel.StartMinutes,
                EndHour = eventViewModel.EndHour,
                EndMinute = eventViewModel.EndMinutes,
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                IdRoom = eventViewModel.IdRoom,
                Nickname = eventViewModel.Nickname,
                Publicity = eventViewModel.Publicity
            };
        }
    }
}