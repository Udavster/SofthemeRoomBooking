using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SofthemeRoomBooking.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Start { get; set; }
        public string Finish { get; set; }
        public bool Publicity { get; set; }
        public int Id_room { get; set; }
        public string Nickname { get; set; }
        public IEnumerable<SelectListItem> Rooms { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
    }
}