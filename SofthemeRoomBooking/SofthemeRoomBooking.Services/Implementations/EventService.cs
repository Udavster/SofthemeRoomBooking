using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Implementations
{
    class EventService : IEventService
    {
        private SofhemeRoomBookingContext _context;
        public EventService(SofhemeRoomBookingContext context)
        {
            _context = context;
        }

        public void AddEvent(EventModel model, string userId)
        {
            DateTime startTime = DateTime.Parse(DateTime.Now.Year+"-"+model.Month+"-"+model.Day+" "+model.StartHour+":"+model.StartMinute);
            DateTime endTime = DateTime.Parse(DateTime.Now.Year+"-"+model.Month +"-"+model.Day+" "+model.EndHour+":"+model.EndMinute);
            Events newEvent = new Events()
            {
                Start = startTime,
                Finish = endTime,
                Title = model.Title,
                Description = model.Description,
                Id_room = model.Id_room,
                Nickname = model.Nickname,
                Publicity = model.Publicity,
                Id_user = userId
            };

            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }
    }
}
