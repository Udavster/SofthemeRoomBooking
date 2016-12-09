using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SofthemeRoomBooking.Models
{
    public class SchedulerEventsViewModel
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
        [JsonProperty(PropertyName = "isPrivate")]
        public bool Publicity { get; set; }
        public int IdRoom { get; set; }
        public string Nickname { get; set; }
    }
}