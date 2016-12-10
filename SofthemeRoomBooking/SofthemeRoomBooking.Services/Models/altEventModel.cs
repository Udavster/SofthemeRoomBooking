using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class altEventModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }

        public bool? Publicity { get; set; }

        public int IdRoom { get; set; }

        //public string IdUser { get; set; }
    }
}

