using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IEventService _eventService;
        public RoomController(IRoomService roomService, IEventService eventService)
        {
            _roomService = roomService;
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult Events(DateTime date, int id)
        {
            var events = _eventService.GetEventsByWeek(date, id);
            var result = JsonConvert.SerializeObject(events);
            return Content(result, "application/json");
        }

        public ActionResult RoomPartial()
        {
            var rooms = _roomService.GetAllEquipmentRooms();

            return PartialView("_RoomsPartial", rooms);
        }
        [HttpGet]
        public ActionResult RoomEquipment(int id)
        {
            var equipmentRoomModel = _roomService.GetEquipmentByRoom(id);
            var equipmentRoomViewModel = equipmentRoomModel.ToRoomEquipmentViewModel();

            return PartialView("_PopupEquipmentAdminPartial", equipmentRoomViewModel);
        }

        [HttpPost]
        public ActionResult ChangeRoomEquipment(RoomEquipmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roomEquipmentModel = model.ToRoomEquipmentModel();
                _roomService.ChangeRoomEquipment(roomEquipmentModel);
                return Redirect("/Home/Index");
            }
            return Redirect("/Home/Index");
        }

    }
}