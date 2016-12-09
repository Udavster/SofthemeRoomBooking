using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SofthemeRoomBooking.Models
{
    public class NewEventViewModel
    {
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public bool Publicity { get; set; }
        public int IdRoom { get; set; }
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