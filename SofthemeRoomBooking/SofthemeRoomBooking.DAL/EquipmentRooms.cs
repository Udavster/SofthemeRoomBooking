namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EquipmentRooms
    {
        public int Id { get; set; }

        public int Id_room { get; set; }

        public int Id_equipment { get; set; }

        public int Quantity { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual Rooms Rooms { get; set; }
    }
}
