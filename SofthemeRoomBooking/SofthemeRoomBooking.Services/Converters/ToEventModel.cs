using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToEventModel
    {
        public static EventRoomModel ToEvent(this Events events)
        {
            return new EventRoomModel()
            {
                Start = events.Start.ToString("yyyy-MM-dd HH:mm"),
                Finish = events.Finish.ToString("yyyy-MM-dd HH:mm"),
                Title = events.Title,
                Description = events.Description,
                Id = events.Id,
                Publicity = events.Publicity
            };
        }
    }
}
