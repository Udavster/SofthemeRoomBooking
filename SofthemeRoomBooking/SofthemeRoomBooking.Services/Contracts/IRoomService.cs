using System;
using SofthemeRoomBooking.Services.Models;
using System.Collections.Generic;
using SofthemeRoomBooking.Services.Models.EventModel;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IRoomService
    {
        List<RoomEquipmentModel> GetAllEquipmentRooms();
        List<RoomModel> GetAllRooms();
        RoomEquipmentModel GetEquipmentByRoom(int roomId);
        bool UpdateRoomEquipment(RoomEquipmentModel model);
        RoomModel[] GetUnlockedRoomsByDate(DateTime date);
        bool CloseRoom(int id, string userId, Dictionary<int, string> creatorsEmails, DateTime? finish = null);
        bool OpenRoom(int id);

        bool IsBusyRoom(int idRoom, DateTime startTime, DateTime finishTime, int? idEvent = null);
    }
}
