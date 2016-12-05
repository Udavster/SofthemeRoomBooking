namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IRoomService
    {
        string GetEventsByWeek(string date,int id);
    }
}
