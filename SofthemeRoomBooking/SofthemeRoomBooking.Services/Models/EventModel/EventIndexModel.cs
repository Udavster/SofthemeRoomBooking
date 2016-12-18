using System;

namespace SofthemeRoomBooking.Services.Models.EventModel
{
    public class EventIndexModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Publicity { get; set; }

        public bool AllowRegistration { get; set; }

        public int ParticipantsQuantity { get; set; }

        public int IdRoom { get; set; }

        public string IdUser { get; set; }

        public string Nickname { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }
    }
}
