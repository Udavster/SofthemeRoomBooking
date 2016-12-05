using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SofthemeRoomBooking.Services.Models
{
    public class Event
    {
        [JsonProperty(PropertyName = "eventId")]
        public int eventId { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public string startDate { get; set; }
        [JsonProperty(PropertyName = "endDate")]
        public string endDate { get; set; }
        [JsonProperty(PropertyName = "isPrivate")]
        public bool isPrivate { get; set; }
    }
}
