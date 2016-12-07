using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class EventConverter
    {
        public static EventModel ToEventModel(this EventViewModel eventViewModel)
        {
            return new EventModel()
            {
                Day = eventViewModel.Day,
                Month = eventViewModel.Month,
                StartHour = eventViewModel.StartHour,
                StartMinute = eventViewModel.StartMinute,
                EndHour = eventViewModel.EndHour,
                EndMinute = eventViewModel.EndMinute,
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                Id_room = eventViewModel.Id_room,
                Nickname = eventViewModel.Nickname,
                Publicity = eventViewModel.Publicity
            };
        }
    }
}