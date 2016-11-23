using System;
using System.ComponentModel.DataAnnotations;

namespace SofthemeRoomBooking.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public String Email { get; set; }

        [DataType(DataType.MultilineText)]
        public String Message { get; set; }

        public DateTime Created { get; set; }
    }
}