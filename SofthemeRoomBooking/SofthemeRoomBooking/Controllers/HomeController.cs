using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Events(string date)
        {
            DateTime dt = DateTime.ParseExact(date,
                                  "yyyy-MM-dd",
                                  CultureInfo.InvariantCulture);
            List<EventsList> events = new List<EventsList>();
            EventsList day1 = new EventsList() { events = new List<Event>() };
            EventsList day2 = new EventsList() { events = new List<Event>() };
            EventsList day3 = new EventsList() { events = new List<Event>() };

        EventsList day4 = new EventsList() { events = new List<Event>() };
        EventsList day5 = new EventsList() { events = new List<Event>() };
        EventsList day6 = new EventsList() { events = new List<Event>() };
        EventsList day7 = new EventsList() { events = new List<Event>() };
        EventsList day8 = new EventsList() { events = new List<Event>() };
        Event event1 = new Event()
        {
            description = "sdfsdfsd",
            endDate = "2016-12-04 13:40",
            eventId = 1,
            isPrivate = true,
            startDate = "2016-12-04 13:00",
            title = "dfsdfsd"
        };
        Event event2 = new Event()
        {
            description = "sdfsdfsd",
            endDate = "2016-12-04 14:40",
            eventId = 1,
            isPrivate = true,
            startDate = "2016-12-04 14:00",
            title = "dfsdfsd"
        };
        Event event3 = new Event()
        {
            description = "sdfsdfsd",
            endDate = "2016-12-04 15:40",
            eventId = 1,
            isPrivate = true,
            startDate = "2016-12-04 15:00",
            title = "dfsdfsd"
        };

        day1.events.Add(event1);
            day1.events.Add(event2);

            day1.events.Add(event3);

            day2.events.Add(event1);
            day2.events.Add(event2);

            day2.events.Add(event3);
            events.Add(day1);
            events.Add(day2);
            events.Add(day3);
            events.Add(day4);
            events.Add(day5);
            events.Add(day6);
            events.Add(day7);
            events.Add(day8);

            return Json(events, JsonRequestBehavior.AllowGet);
    }
}
}