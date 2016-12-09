using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SofthemeRoomBooking.Models
{
    public class CalendarEventModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeCalendar Start { get; set; }

        public TimeCalendar? Finish { get; set; }

        public bool? Publicity { get; set; }

        //public string RoomName { get; set; }
    }

    public struct TimeCalendar
    {
        public int h;
        public int m;
    }
}