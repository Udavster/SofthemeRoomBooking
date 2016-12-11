using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public ActionResult Events(string date, int id)
        {
            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }
            var result = _roomService.GetEventsByWeek(date, id);
            return Content(result, "application/json");
        }

    }
}