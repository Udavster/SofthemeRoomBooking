using System;
using SofthemeRoomBooking.Services.Models;


namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IRoomService
    {
        string GetEventsByWeek(string date,int id);
        RoomModel[] GetUnlockedRoomsByDate(DateTime date);
    }
}
