using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IRoomService
    {
        string GetEventsByWeek(string date,int id);
    }
}
