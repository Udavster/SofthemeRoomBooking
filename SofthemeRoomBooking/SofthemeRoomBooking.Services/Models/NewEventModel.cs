using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class NewEventModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Publicity { get; set; }
        public int IdRoom { get; set; }
        public string Nickname { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
    }
}
