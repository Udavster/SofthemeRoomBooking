using System;
using System.ComponentModel.DataAnnotations;

namespace SofthemeRoomBooking.Models.EventViewModel
{
    public class EventIndexViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Заголовок не может быть длиннее 50 символов")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Дополнительная информация не может быть длиннее 500 символов")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} поле обязательно для заполнения")]
        public string Organizator { get; set; }

        [StringLength(150, ErrorMessage = "Имя организатора не может быть длиннее 150 символов")]
        public string Nickname { get; set; }

        public bool Private { get; set; }

        public bool AllowRegistration { get; set; }

        public int ParticipantsQuantity { get; set; }

        public int IdRoom { get; set; }

        public string IdUser { get; set; }

        public bool IsAdminOrOrganizator { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int StartHour { get; set; }

        public int StartMinutes { get; set; }

        public int FinishHour { get; set; }

        public int FinishMinutes { get; set; }
    }
}