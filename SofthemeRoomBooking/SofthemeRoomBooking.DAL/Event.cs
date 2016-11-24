namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Event")]
    public partial class Event
    {
        public int id { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public int? id_user { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? start { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? finish { get; set; }

        public bool? publicity { get; set; }
    }
}
