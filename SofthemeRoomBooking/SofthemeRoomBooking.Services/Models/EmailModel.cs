using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class EmailModel
    {
        public List<string> Emails { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }
    }
}
