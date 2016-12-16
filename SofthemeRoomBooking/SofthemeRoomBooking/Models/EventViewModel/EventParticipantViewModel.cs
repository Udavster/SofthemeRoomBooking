using System.ComponentModel.DataAnnotations;

namespace SofthemeRoomBooking.Models.EventViewModel
{
    public class EventParticipantViewModel
    {
        public int Id { get; set; }

        public int IdEvent { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", ErrorMessage = "Неверный адрес электронной почты")]
        public string Email { get; set; }
    }
}