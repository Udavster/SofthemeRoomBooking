using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToEventDetailsModel
    {
        public static EventDetailsModel ToEventUserModelConverter(this Events events, int participantsQuantity,string[] dayOfWeek, string[] month)
        {
            return new EventDetailsModel()
            {
                Id = events.Id,
                Title = events.Title,
                Description = events.Description,
                Nickname = events.Nickname,
                DayOfWeek = dayOfWeek[(int)events.Start.DayOfWeek],
                Month = month[events.Start.Month-1],
                Start = events.Start,
                Finish = events.Finish,
                ParticipantsQuantity = participantsQuantity,
                UserId = events.Id_user
            };
        }
    }
}
