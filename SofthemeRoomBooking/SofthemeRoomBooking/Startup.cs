﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SofthemeRoomBooking.Startup))]
namespace SofthemeRoomBooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = SimpleInjectorConfiguration.Initialize(app);
            ConfigureAuth(app,container);
        }
    }
}
