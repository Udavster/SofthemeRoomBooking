namespace SofthemeRoomBooking.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SofhemeRoomBookingContext : DbContext
    {
        public SofhemeRoomBookingContext()
            : base("name=SofhemeRoomBookingContext")
        {
        }

        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentRooms> EquipmentRooms { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<RoomsLocks> RoomsLocks { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.EquipmentRooms)
                .WithRequired(e => e.Equipment)
                .HasForeignKey(e => e.Id_equipment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rooms>()
                .HasMany(e => e.EquipmentRooms)
                .WithRequired(e => e.Rooms)
                .HasForeignKey(e => e.Id_room)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rooms>()
                .HasMany(e => e.Events)
                .WithRequired(e => e.Rooms)
                .HasForeignKey(e => e.Id_room)
                .WillCascadeOnDelete(false);
        }
    }
}
