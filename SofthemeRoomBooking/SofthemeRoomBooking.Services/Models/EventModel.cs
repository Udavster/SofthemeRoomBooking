﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SofthemeRoomBooking.Services.Models
{
    public class EventModel
    {
        [JsonProperty(PropertyName = "eventId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public string Start { get; set; }
        [JsonProperty(PropertyName = "endDate")]
        public string Finish { get; set; }
        [JsonProperty(PropertyName = "isPublic")]
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
