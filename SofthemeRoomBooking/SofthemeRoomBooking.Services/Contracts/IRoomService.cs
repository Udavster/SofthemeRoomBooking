using System;
using SofthemeRoomBooking.Services.Models;
using System.Collections.Generic;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IRoomService
    {
        List<RoomEquipmentModel> GetAllEquipmentRooms();
        List<RoomModel> GetAllRooms();
        RoomEquipmentModel GetEquipmentByRoom(int roomId);
        void ChangeRoomEquipment(RoomEquipmentModel model);
        RoomModel[] GetUnlockedRoomsByDate(DateTime date);
    }
}
