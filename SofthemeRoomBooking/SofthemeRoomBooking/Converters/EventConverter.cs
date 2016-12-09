using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class EventConverter
    {
        public static NewEventModel ToNewEventModel(this NewEventViewModel eventViewModel)
        {
            return new NewEventModel()
            {
                Day = eventViewModel.Day,
                Month = eventViewModel.Month,
                StartHour = eventViewModel.StartHour,
                StartMinute = eventViewModel.StartMinute,
                EndHour = eventViewModel.EndHour,
                EndMinute = eventViewModel.EndMinute,
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                IdRoom = eventViewModel.IdRoom,
                Nickname = eventViewModel.Nickname,
                Publicity = eventViewModel.Publicity
            };
        }
    }
}