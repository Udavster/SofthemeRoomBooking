﻿using SimpleInjector;
using SimpleInjector.Packaging;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Implementations;

namespace SofthemeRoomBooking.Services.Package
{
    public class ServicesPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IRoomService,RoomService>();
            //container.RegisterSingleton<SofhemeRoomBookingContext>();
            container.Register<IEventService, EventService>();
            container.Register<ICalendarService, CalendarService>();
            container.RegisterSingleton<SofhemeRoomBookingContext>();
        }
    }
}