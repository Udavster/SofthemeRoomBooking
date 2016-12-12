namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoomsLock
    {
        public int Id { get; set; }

        public int IdRoom { get; set; }

        [Required]
        [StringLength(128)]
        public string IdUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Start { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Finish { get; set; }
    }
}
