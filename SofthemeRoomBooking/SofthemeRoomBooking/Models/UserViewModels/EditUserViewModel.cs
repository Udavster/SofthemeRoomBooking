using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SofthemeRoomBooking.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Имя не может быть длиннее 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Фамилия не может быть длиннее 50 символов")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [RegularExpression(@"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$", ErrorMessage = "Неверный адрес электронной почты")]
        public string Email { get; set; }

        public bool AdminRole { get; set; }
    }
}