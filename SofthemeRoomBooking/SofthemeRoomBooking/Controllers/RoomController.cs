﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class RoomController : ErrorCatchingControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IEventService _eventService;
        private readonly IProfileService _profileService;
        public RoomController(IRoomService roomService, IEventService eventService, IProfileService profileService)
        {
            _roomService = roomService;
            _eventService = eventService;
            _profileService = profileService;
        }

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Events(DateTime date, int id)
        {
            var events = _eventService.GetEventsByWeek(date, id);
            var result = JsonConvert.SerializeObject(events);
            return Content(result, "application/json");
        }

        public ActionResult RoomPartial(int? id)
        {
            var rooms = _roomService.GetAllEquipmentRooms();
            ViewBag.RoomId = id;
            if (id != null)
            {
                var isRoomExist = rooms.Any(x=>x.Id ==id);
                if (!isRoomExist)
                {
                    throw new HttpException(404, "Not found");
                }
                if (!User.IsInRole("Admin") && !rooms.FirstOrDefault(x => x.Id == id).IsAvalaible)
                {
                    throw new HttpException(401, "Unauthorized");
                }
            }
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
        public ActionResult UpdateRoomEquipment(RoomEquipmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roomEquipmentModel = model.ToRoomEquipmentModel();
                _roomService.UpdateRoomEquipment(roomEquipmentModel);
                return Redirect("/Home/Index");
            }
            return Redirect("/Home/Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Open(string id)
        {
            if (id != null)
            {
                var model = new ConfirmationViewModel
                {
                    Question = "Вы уверены, что хотите открыть эту комнату?",
                    Message = "",
                    Action = "Open",
                    Controller = "Room",
                    DataId = id
                };

                return PartialView("_PopupConfirmationPartial", model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Close(string id)
        {
            if (id != null)
            {
                var model = new ConfirmationViewModel
                {
                    Question = "Вы уверены, что хотите закрыть эту комнату?",
                    Message = "Все события,запланированные в данной аудитории, будут отменены.",
                    Action = "Close",
                    Controller = "Room",
                    DataId = id
                };

                return PartialView("_PopupConfirmationPartial", model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ActionName("Open")]
        public ActionResult OpenRoomConfirmation(int id)
        {
         
            _roomService.OpenRoom(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ActionName("Close")]
        public ActionResult CloseRoomConfirmation(int id)
        {
            var userId = User.Identity.GetUserId();
            var events = _eventService.GetEventsByRoom(id);
            Dictionary<int,string> creatorEmailByEvent = new Dictionary<int, string>();
            foreach (var @event in events)
            {
                creatorEmailByEvent.Add(@event.Id,_profileService.GetUserById(@event.UserId).Email);
            }
            _roomService.CloseRoom(id, userId, creatorEmailByEvent);

            return RedirectToAction("Index", "Home");
        }

    }
}