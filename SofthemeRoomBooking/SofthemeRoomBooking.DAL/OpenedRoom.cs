using System.Data;

namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OpenedRooms")]
    public partial class OpenedRoom
    {
        public int Id { get; set; }

        [StringLength(35)]
        public string Name { get; set; }
    }
}
