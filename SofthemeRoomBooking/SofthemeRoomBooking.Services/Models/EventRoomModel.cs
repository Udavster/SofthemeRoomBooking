using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SofthemeRoomBooking.Services.Models
{
    public class EventRoomModel
    {
        [JsonProperty(PropertyName = "eventId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public string Start { get; set; }
        [JsonProperty(PropertyName = "endDate")]
        public string Finish { get; set; }
        [JsonProperty(PropertyName = "isPublic")]
        public bool Publicity { get; set; }
    }
}
