using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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

        public List<List<EventRoomModel>> GetEventsByWeek(DateTime date, int id)
        {

            List<List<EventRoomModel>> events = new List<List<EventRoomModel>>();
            var endTime  = date.Date.Add(TimeSpan.FromDays(10));
            var event1 =
                _context.Events.Where(
                    ev => DbFunctions.TruncateTime(ev.Start) >= DbFunctions.TruncateTime(date) &&
                    DbFunctions.TruncateTime(ev.Start) <= DbFunctions.TruncateTime(endTime)
                    && ev.Id_room == id);
            var currentDate = date;
            for (int i = 0; i < 8; i++)
            {
                List<EventRoomModel> day = new List<EventRoomModel>();
                if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    events.Add(day);
                    currentDate = currentDate.AddDays(1);
                    continue;
                }
                if (currentDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    events.Add(day);
                   currentDate = currentDate.AddDays(2);
                    continue;
                }
                var currDay = event1.Where(ev => ev.Start.Day == currentDate.Day);
                foreach (var eventItem in currDay)
                {
                    day.Add(eventItem.ToEvent());
                }
                events.Add(day);
                currentDate = currentDate.AddDays(1);
            }

            return events;
        }

        public List<RoomEquipmentModel> GetAllEquipmentRooms()
        {
        List<RoomEquipmentModel> listRoomEq = new List<RoomEquipmentModel>();
        var now = DateTime.Now;
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
                        ((roomLock.Finish == null) && (roomLock.Start >= now)) || ((roomLock.Finish != null) && (now > roomLock.Finish)),
                        Equipment = _context.EquipmentRooms.Select(x => x).Where(x => x.Id_room == room.Id)
                    };

        var result = query.GroupBy(q => new { Id = q.Id, Name = q.Name })
                        .Select(group => new
                        {
                            Id = group.Key.Id,
                            Name = group.Key.Name,
                            IsAvailable = group.All(q => q.IsAvailable),
                            Equipment = group.FirstOrDefault().Equipment
                        }
                    );

            RoomEquipmentModel model;
            foreach (var item in result)
            {
                var equipment = "";
                foreach (var i in item.Equipment)
                {
                    equipment += i.Quantity + ",";
                }
                equipment = equipment.TrimEnd(',');
                model = new RoomEquipmentModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsAvalaible = item.IsAvailable,
                    Equipments = equipment
                };
                listRoomEq.Add(model);
            }
            return listRoomEq;
        }

        public RoomEquipmentModel GetEquipmentByRoom(int roomId)
        {
            var result = _context.Rooms.Where(r => r.Id == roomId).Select(room => new
            {
                Equipment = _context.EquipmentRooms.Select(x => x).Where(x => x.Id_room == room.Id),
                room.Name
            }).FirstOrDefault();
            List<EquipmentModel> equip = new List<EquipmentModel>();

            if (result != null)
            {
                foreach (var i in result.Equipment)
                {
                    equip.Add(new EquipmentModel()
                    {
                        Id = i.Id,
                        Quantity = i.Quantity
                    });
                }
            }

            RoomEquipmentModel equipmentModel = new RoomEquipmentModel()
            {
                Id = roomId,
                Name = result.Name,
                Equipment = equip
            };
            return equipmentModel;
        }

        public List<RoomModel> GetAllRooms()
        {
            List<RoomModel> roomsList = new List<RoomModel>(); 
            var rooms = _context.Rooms;
            foreach (var roomEntity in rooms)
            {
                roomsList.Add(roomEntity.ToRoom());
            }
            return roomsList;
        }

        public void ChangeRoomEquipment(RoomEquipmentModel model)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == model.Id);
            var equipment = _context.EquipmentRooms.Where(x => x.Id_room == model.Id);

            if (room != null)
            {
                room.Name = model.Name;
            }
            var i = 0;
            foreach (var item in equipment)
            {
                item.Quantity = model.Equipment[i].Quantity;
                i++;
            }
            _context.SaveChanges();
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