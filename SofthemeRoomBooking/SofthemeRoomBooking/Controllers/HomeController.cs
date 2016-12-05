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
        public ActionResult Events()
        {
            //DateTime dt = DateTime.ParseExact(date,
            //                      "yyyy-MM-dd",
            //                      CultureInfo.InvariantCulture);
         //   List<EventsList> events = new List<EventsList>();

        //EventsList day4 = new EventsList() { events = new List<Event>() };
        //EventsList day5 = new EventsList() { events = new List<Event>() };
        //EventsList day6 = new EventsList() { events = new List<Event>() };
        //EventsList day7 = new EventsList() { events = new List<Event>() };
        //EventsList day8 = new EventsList() { events = new List<Event>() };
        Event event1 = new Event()
        {
            Description = "sdfsdfsd",
            Finish = "2016-12-04 15:40",
            Id = 1,
            Publicity = true,
            Start = "2016-12-04 15:00",
            Title = "dfsdfsd"
        };
        Event event2 = new Event()
        {
            Description = "sdfsdfsd",
            Finish = "2016-12-04 14:40",
            Id = 1,
            Publicity = true,
            Start = "2016-12-04 14:00",
            Title = "dfsdfsd"
        };
        Event event3 = new Event()
        {
            Description = "sdfsdfsd",
            Finish = "2016-12-04 13:40",
            Id = 1,
            Publicity = true,
            Start = "2016-12-04 13:00",
            Title = "dfsdfsd"
        };
        List<Event> day1 = new List<Event>();
        List<Event> day2 = new List<Event>();
        List<Event> day3 = new List<Event>();
        List<Event> day4 = new List<Event>();
        List<Event> day5 = new List<Event>();
        List<Event> day6 = new List<Event>();
        List<Event> day7 = new List<Event>();
        List<Event> day8 = new List<Event>();
        List<List<Event>> events = new List<List<Event>>();

            day1.Add(event1);
            day1.Add(event2);
            day1.Add(event3);

            day2.Add(event1);
            day2.Add(event2);

            day2.Add(event3);
            events.Add(day1);
            events.Add(day2);
            events.Add(day3);
            events.Add(day4);
            events.Add(day5);
            events.Add(day6);
            events.Add(day7);
            events.Add(day8);
            var result = JsonConvert.SerializeObject(events);

            return Content(result, "application/json");
    }
}
}