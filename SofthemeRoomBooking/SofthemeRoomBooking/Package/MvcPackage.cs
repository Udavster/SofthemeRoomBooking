﻿using SimpleInjector;
using SimpleInjector.Packaging;
using SofthemeRoomBooking.Services;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Implementation;
using SofthemeRoomBooking.Services.Implementations;

namespace SofthemeRoomBooking.Package
{
    public class MvcPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IProfileService, ProfileService>();
        }
    }
}