using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Id_room = roomEntity.Id,
                Name = roomEntity.Name
            };
        }
    }
}
