using System;

namespace SofthemeRoomBooking.Models
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Nickname { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int ParticipantsQuantity { get; set; }

        public string DayOfWeek { get; set; }

        public string Month { get; set; }

        public string UserId { get; set; }

        public bool Publicity { get; set; }

        public bool AllowRegistration { get; set; }

        public string UserName { get; set; }
    }
}