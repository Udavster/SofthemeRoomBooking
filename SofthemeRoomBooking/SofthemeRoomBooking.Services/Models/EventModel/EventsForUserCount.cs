﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models.EventModel
{
    public struct EventsForUserCount
    {
        public string UserId { get; set; }
        public int EventCount { get; set; }
    }
}