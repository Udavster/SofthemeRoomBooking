using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IEventService
    {
        void AddEvent(EventModel model, string userId);
        altEventModel[] GetEventsByDate(string date);
    }
}
