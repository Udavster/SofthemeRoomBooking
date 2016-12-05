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
            Events newEvent = new Events()
            {
                Start = DateTime.Parse(model.Start),
                Finish = DateTime.Parse(model.Finish),
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
