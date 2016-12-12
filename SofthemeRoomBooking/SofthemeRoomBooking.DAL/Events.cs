namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Events
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string Id_user { get; set; }

        [StringLength(150)]
        public string Nickname { get; set; }

        public int Id_room { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Start { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Finish { get; set; }

        public bool Publicity { get; set; }

        public bool Cancelled { get; set; }

        public virtual Rooms Rooms { get; set; }
    }
}
