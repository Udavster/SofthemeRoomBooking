namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EquipmentRoom
    {
        public int id { get; set; }

        public int? id_room { get; set; }

        public int? id_equipment { get; set; }

        public int? quantity { get; set; }
    }
}
