using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class FeedbackModel
    {
        public String Name { get; set; }

        public String Surname { get; set; }

        public String Email { get; set; }

        public String Message { get; set; }

        public DateTime Created { get; set; }
    }

}
