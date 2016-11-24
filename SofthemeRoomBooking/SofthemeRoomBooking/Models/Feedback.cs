using System;
using System.ComponentModel.DataAnnotations;

namespace SofthemeRoomBooking.Models
{
    public class Feedback
    {
        public Guid Id { get; set; }

        [StringLength(3)]
        [Required(ErrorMessage = "This field is really required!")]
        public String Name { get; set; }

        [Required(ErrorMessage = "This field is really required!")]
        [StringLength(3)]
        public String Surname { get; set; }

        [Required(ErrorMessage = "This field is really required!")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Your password is too big or too short.")]
        public String Email { get; set; }

        [Required(ErrorMessage = "This field is really required!")]
        [DataType(DataType.MultilineText)]
        public String Message { get; set; }

        [Required(ErrorMessage = "This field is really required!")]
        public DateTime Created { get; set; }
    }
}