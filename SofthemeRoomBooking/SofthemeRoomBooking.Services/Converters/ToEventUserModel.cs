using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToEventUserModel
    {
        public static EventUserModel ToEventUserModelConverter(this Events events, int participantsQuantity,string[] dayOfWeek, string[] month)
        {
            return new EventUserModel()
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
