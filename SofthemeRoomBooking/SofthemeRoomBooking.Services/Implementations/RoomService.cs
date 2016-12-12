using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Converters;
using SofthemeRoomBooking.Services.Models;


namespace SofthemeRoomBooking.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private SofhemeRoomBookingContext _context;
        public RoomService(SofhemeRoomBookingContext context)
        {
            _context = context;
        }

        public string GetEventsByWeek(string date, int id)
        {
            return null;
        }

        public RoomModel[] GetUnlockedRoomsByDate(DateTime date)
        {
            var day = date.Date;
            var nextDay = day.AddDays(1);

            var rooms = from room in _context.Rooms
                        join roomLock in _context.RoomsLocks
                        on room.Id equals roomLock.IdRoom into wh
                        from roomLock in wh.DefaultIfEmpty()
                        where (roomLock == null) || roomLock.Finish < nextDay || (roomLock.Start > day) 
                        select new RoomModel() {Id = room.Id, Name = room.Name};

            return rooms.ToArray();
        }

        public IEnumerable<RoomStatus> GetRoomsStatuses(DateTime now)
        {
            var query = from room in _context.Rooms
                     join roomLock in _context.RoomsLocks
                     on room.Id equals roomLock.IdRoom into wh
                     from roomLock in wh.DefaultIfEmpty()
                     select
                     new
                     {
                         Id = room.Id,
                         Name = room.Name,
                         IsAvailable =
                         (roomLock == null) ||
                         ((roomLock.Finish == null) && (roomLock.Start >= now)) || ((roomLock.Finish != null) && (now > roomLock.Finish))
                     };

            var result = query.GroupBy(q => new { Id = q.Id, Name = q.Name})
                               .Select(group => 
                                        new RoomStatus
                                        {
                                            IdRoom = group.Key.Id,
                                            Name = group.Key.Name,
                                            IsAvailable = group.All(q => q.IsAvailable)
                                        }
                                       );

            return result;
        }

    }
}