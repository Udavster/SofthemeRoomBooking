using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class EventUserModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Nickname { get; set; }
        public DateTime Start  { get; set; }
        public DateTime Finish { get; set; }
        public int ParticipantsQuantity { get; set; }
        public string DayOfWeek { get; set; }
        public string Month { get; set; }
        public string UserId { get; set; }
    }
}
