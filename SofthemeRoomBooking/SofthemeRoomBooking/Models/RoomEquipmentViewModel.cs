using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Models
{
    public class RoomEquipmentViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Название не может быть больше 20 символов")]
        public string Name { get; set; }
        public bool IsAvalaible { get; set; }
        public List<EquipmentModel> Equipment { get; set; }
        public string Equipments { get; set; }
    }
}