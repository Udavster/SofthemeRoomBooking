using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SofthemeRoomBooking.DAL.Package;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Package;

namespace SofthemeRoomBooking
{
    public static class SimpleInjectorConfiguration
    {
        public static Container Initialize(IAppBuilder app)
        {
            var container = GetInitializeContainer(app);
            container.RegisterPackages(
                new[]
                    {
                        typeof(ServicesPackage).Assembly, typeof(DalPackage).Assembly
                    });
       //     container.Verify();

            DependencyResolver.SetResolver(
                new SimpleInjectorDependencyResolver(container));

            return container;
        }
        public static Container GetInitializeContainer(
                 IAppBuilder app)
        {
            var container = new Container();

            container.RegisterSingleton<IAppBuilder>(app);

            container.RegisterPerWebRequest<
                   ApplicationUserManager>();

            container.RegisterPerWebRequest<ApplicationDbContext>(()
              => new ApplicationDbContext(
               "SofthemeRoomBooking"));

            container.RegisterPerWebRequest<IUserStore<
                ApplicationUser>>(() =>
                new UserStore<ApplicationUser>(
                  container.GetInstance<ApplicationDbContext>()));

            container.RegisterInitializer<ApplicationUserManager>(
                manager => InitializeUserManager(manager, app));

            container.RegisterMvcControllers(
                    Assembly.GetExecutingAssembly());
            container.RegisterPerWebRequest<SignInManager<ApplicationUser, string>, ApplicationSignInManager>();

            container.RegisterPerWebRequest<IAuthenticationManager>(() =>
            AdvancedExtensions.IsVerifying(container)
            ? new OwinContext(new Dictionary<string, object>()).Authentication
            : HttpContext.Current.GetOwinContext().Authentication);

            return container;
        }

        private static void InitializeUserManager(
            ApplicationUserManager manager, IAppBuilder app)
        {
            manager.UserValidator =
             new UserValidator<ApplicationUser>(manager)
             {
                 AllowOnlyAlphanumericUserNames = false,
                 RequireUniqueEmail = true
             };

            //Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            var dataProtectionProvider =
                 app.GetDataProtectionProvider();

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                 new DataProtectorTokenProvider<ApplicationUser>(
                  dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}