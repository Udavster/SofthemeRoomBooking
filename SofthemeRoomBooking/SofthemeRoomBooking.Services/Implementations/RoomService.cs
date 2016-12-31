using System;
using System.Collections.Generic;
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
        private INotificationService _notificationService;
        public RoomService(SofhemeRoomBookingContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
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

        public bool UpdateRoomEquipment(RoomEquipmentModel model)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == model.Id);
            var equipment = _context.EquipmentRooms.Where(x => x.Id_room == model.Id);

            if (room != null)
            {
                room.Name = model.Name;
                var i = 0;
                foreach (var item in equipment)
                {
                    item.Quantity = model.Equipment[i].Quantity;
                    i++;
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public RoomModel[] GetUnlockedRoomsByDate(DateTime date)
        {
            var day = date.Date;
            var nextDay = day.AddDays(1);


            var rooms = from room in _context.Rooms
                        where !(from RL in _context.RoomsLocks
                            where (RL.Start < day) && ((RL.Finish == null) || (RL.Finish >= nextDay))
                            select RL.IdRoom).Contains(room.Id)
                        select room;

            var r = rooms.GroupBy(room => new { room.Id, room.Name}).Select(group=>new RoomModel() { Id = group.Key.Id, Name = group.Key.Name });

            return r.ToArray();
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

        public bool CloseRoom(int id, string userId, DateTime? finish = null)
        {
            if (userId != null)
            {
                var room = new RoomsLocks()
                {
                    IdRoom = id,
                    IdUser = userId,
                    Start = DateTime.Now.AddMinutes(-1),
                    Finish = finish
                };

                _context.RoomsLocks.Add(room);
                var events = GetEventsToCancel(id,finish);
                foreach (var @event in events)
                {
                    @event.Cancelled = true;
                }

                _context.SaveChanges();

                foreach (var @event in events)
                {
                    var usersEmails = _context.EventsUsers.Where(ev => ev.IdEvent == @event.Id).Select(x => x.Email).ToList();
                    var eventInfo = _context.Events.FirstOrDefault(ev => ev.Id == @event.Id);

                   _notificationService.CancelEventNotification(usersEmails,eventInfo);
                }

                return true;
            }
            return false;
        }

        public bool OpenRoom(int id)
        {
            var room = _context.RoomsLocks.Where(r => r.IdRoom == id && (r.Finish == null || r.Finish > DateTime.Now));
            if (room.Any())
            {
                foreach (var item in room)
                {
                    item.Finish = DateTime.Now.AddMinutes(-1); ;
                }

                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsBusyRoom(int idRoom, DateTime startTime, DateTime finishTime, int? idEvent = null)
        {
            var result = _context.Events.Where(ev => ev.Id_room == idRoom && !ev.Cancelled &&
                                              ((ev.Start > startTime && ev.Start < finishTime) ||
                                               (ev.Finish > startTime && ev.Finish < finishTime)));

            if (idEvent != null)
            {
                return result.Count(ev => ev.Id != idEvent.Value) > 0;
            }

            return result.Any();
        }

        private IQueryable<Events> GetEventsToCancel(int id, DateTime? finish)
        {
            var now = DateTime.Now;
            if (finish == null)
            {
                return _context.Events.Where(ev => (ev.Id_room == id) && (!ev.Cancelled)
                                            && ((ev.Start >= now)
                                            || (ev.Finish >= now)
                                            || (ev.Start <=now && ev.Finish >= now)) );
            }
            return _context.Events.Where(ev => (ev.Id_room == id ) 
                                        && (!ev.Cancelled)
                                        && ((ev.Start >= now && ev.Start <= finish) 
                                        ||(ev.Finish >= now && ev.Finish <= finish)
                                        || (ev.Start <= now) && ev.Finish >= now));
        }
    }
}
