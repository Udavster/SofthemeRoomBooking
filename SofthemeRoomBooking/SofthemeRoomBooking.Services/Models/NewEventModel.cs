using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class NewEventModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool Publicity { get; set; }

        public int IdRoom { get; set; }

        public string Nickname { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
