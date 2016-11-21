using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;
using SimpleInjector.Packaging;
using SofthemeRoomBooking.Models;
using Microsoft.AspNet.Identity.Owin;

namespace SofthemeRoomBooking
{
    public class MvcPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            //container.Register<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(Lifestyle.Scoped);
            //container.Register<UserManager<ApplicationUser>>(Lifestyle.Scoped);
            //container.RegisterPerWebRequest<ApplicationSignInManager>(() => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());
            //container.Register<ApplicationUserManager>(Lifestyle.Scoped);
        }
    }
}