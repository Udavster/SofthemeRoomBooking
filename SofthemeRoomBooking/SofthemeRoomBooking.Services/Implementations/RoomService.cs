using System;
using System.Linq;
using Newtonsoft.Json;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;


namespace SofthemeRoomBooking.Services.Implementations
{
    public class RoomService : IRoomService
    {
        public void GetEventsByWeek(DateTime time)
        {
            //SofhemeRoomBookingContext context = new SofhemeRoomBookingContext();
            //EventsList events = new EventsList();
            //Event event1 = new Event()
            //{
            //    Description = "sdfsdfsd",
            //    Finish = "2016-12-04 15:40",
            //    Id = 1,
            //    Publicity = true,
            //    Start = "2016-12-04 15:00",
            //    Title = "dfsdfsd"
            //};
            //Event event2 = new Event()
            //{
            //    Description = "sdfsdfsd",
            //    Finish = "2016-12-04 13:00",
            //    Id = 2,
            //    Publicity = true,
            //    Start = "2016-12-04 13:20",
            //    Title = "dfsdfsd"
            //};
            //Event event3 = new Event()
            //{
            //    Description = "sdfsdfsd",
            //    Finish = "2016-12-04 14:00",
            //    Id = 3,
            //    Publicity = true,
            //    Start = "2016-12-04 14:30",
            //    Title = "dfsdfsd"
            //};

            //events.Events.Add(event1);

            //events.Events.Add(event2);

            //events.Events.Add(event3);

           // var result = JsonConvert.SerializeObject(events);

        }
    }
}