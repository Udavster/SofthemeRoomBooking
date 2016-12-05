using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SofthemeRoomBooking.Services.Models
{
    public class EventsList
    {
        [JsonProperty(PropertyName = "events")]
        public List<Event> events { get; set; }
    }
}
