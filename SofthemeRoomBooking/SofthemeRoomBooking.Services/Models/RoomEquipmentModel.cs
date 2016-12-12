using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Models
{
    public class RoomEquipmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvalaible { get; set; }
        public List<EquipmentModel> Equipment { get; set; }
        public string Equipments { get; set; }
    }

    public class EquipmentModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
