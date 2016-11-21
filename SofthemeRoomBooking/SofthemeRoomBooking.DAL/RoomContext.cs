namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RoomContext : DbContext
    {
        public RoomContext()
            : base("name=RoomContext")
        {
        }

        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentRoom> EquipmentRooms { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
