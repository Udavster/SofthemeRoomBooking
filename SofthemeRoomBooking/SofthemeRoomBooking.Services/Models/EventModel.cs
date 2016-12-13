﻿using System;

namespace SofthemeRoomBooking.Services.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Publicity { get; set; }

        public int IdRoom { get; set; }

        public string Nickname { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
