using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class ToRoomModel
    {
        public static RoomModel ToRoom(this Rooms roomEntity)
        {
            return new RoomModel()
            {
                Id = roomEntity.Id,
                Name = roomEntity.Name
            };
        }
    }
}
