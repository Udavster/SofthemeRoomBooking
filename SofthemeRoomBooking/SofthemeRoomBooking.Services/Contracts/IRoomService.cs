using System;
using SofthemeRoomBooking.Services.Models;
using System.Collections.Generic;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IRoomService
    {
        string GetEventsByWeek(DateTime date,int id);
        List<RoomModel> GetAllRooms();
    }
}
