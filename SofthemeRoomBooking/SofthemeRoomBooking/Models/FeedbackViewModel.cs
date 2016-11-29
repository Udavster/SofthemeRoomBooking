using System;
using System.ComponentModel.DataAnnotations;

namespace SofthemeRoomBooking.Models
{
    public class FeedbackViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Имя не может быть длиннее 50 символов")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Фамилия не может быть длиннее 50 символов")]
        public String Surname { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(256, MinimumLength = 6, ErrorMessage = "Email не может быть короче 6 или длиннее 256 символов")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(500, ErrorMessage = "Сообщение не может быть длиннее 500 символов")]
        [DataType(DataType.MultilineText)]
        public String Message { get; set; }

        public DateTime Created { get; set; }
    }
}