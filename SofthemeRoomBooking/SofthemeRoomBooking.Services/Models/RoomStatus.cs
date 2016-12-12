using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public struct RoomStatus
    {
        public int IdRoom { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
    }
}
