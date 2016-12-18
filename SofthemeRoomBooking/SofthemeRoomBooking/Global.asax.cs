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
using Logger = SofthemeRoomBooking.Services.Logger;


namespace SofthemeRoomBooking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //InitializeDataBase();
            //InsertUsers(100);

            Logger.InitLogger();//инициализация - требуется один раз в начале
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

        private void InsertUsers(int count)
        {
            var dbContext = new ApplicationDbContext("SofthemeRoomBooking");

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));

            for (int i = 0; i < count; i++)
            {
                var admin = new ApplicationUser
                {
                    Name = $"User {i}",
                    Surname = "Surname",
                    Email = $"mail{i}@admin.com",
                    UserName = $"mail{i}@admin.com"
                };
                string password = "123qweQ!";

                var result = userManager.Create(admin, password);

                if (!result.Succeeded)
                {
                    break;
                }
            }

            dbContext.Dispose();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();        

            Response.Clear();

            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            bool log = true;
            if (httpException == null)
            {
                routeData.Values.Add("action", "Error");
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        log = false;
                        routeData.Values.Add("action", "NotFound");
                        break;

                    case 500:
                        routeData.Values.Add("action", "Error");
                        break;

                    default:
                        routeData.Values.Add("action", "Error");
                        break;
                }
            }

            if (log)
            {
                string logMessage = String.Format("{0}: {1}", DateTime.Now, exception.ToString());
                Logger.Log.Error(logMessage);
            }

            //routeData.Values.Add("error", exception);
            
            Server.ClearError();

            Response.TrySkipIisCustomErrors = true;

            IController errorController = new SofthemeRoomBooking.Controllers.ErrorController();
            Response.ContentType = "text/html";
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }
    }
}
