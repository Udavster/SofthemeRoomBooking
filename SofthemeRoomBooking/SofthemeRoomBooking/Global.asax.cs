using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector.Integration.Web.Mvc;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //InitializeDataBase();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void InitializeDataBase()
        {
            var dbContext = new ApplicationDbContext("SofthemeRoomBooking");

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            var adminRole = new IdentityRole { Name = "Admin" };

            roleManager.Create(adminRole);

            var admin = new ApplicationUser
            {
                Name = "Super",
                Surname = "User",
                Email = "admin@admin.com",
                UserName = "admin@admin.com"
            };
            string password = "Admin123Q!";

            var result = userManager.Create(admin, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, adminRole.Name);
            }

            dbContext.Dispose();
        }
    }
}
