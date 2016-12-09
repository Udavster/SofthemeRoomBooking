using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface ICalendarService
    {
        altEventModel[] GetEventsByDate(string dateString);
    }
}
