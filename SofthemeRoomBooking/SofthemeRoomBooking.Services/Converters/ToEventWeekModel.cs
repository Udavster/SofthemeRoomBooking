using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToEventWeekModel
    {
        public static EventWeekModel ToEvent(this Events events)
        {
            return new EventWeekModel()
            {
                Id = events.Id,
                Title = events.Title,
                Description = events.Description,              
                Publicity = events.Publicity,
                Start = events.Start.ToString("yyyy-MM-dd HH:mm"),
                Finish = events.Finish.ToString("yyyy-MM-dd HH:mm")
            };
        } 
    }
}
