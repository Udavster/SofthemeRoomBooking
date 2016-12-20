using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SofthemeRoomBooking.Models
{
    public class EquipmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(0,99,ErrorMessage = "Количество должно быть от 0 до 99")]
        public int Quantity { get; set; }
    }
}