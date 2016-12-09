namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OpenedRoomsView : DbContext
    {
        public OpenedRoomsView()
            : base("name=OpenedRooms1")
        {
        }

        public virtual DbSet<OpenedRoom> OpenedRooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
