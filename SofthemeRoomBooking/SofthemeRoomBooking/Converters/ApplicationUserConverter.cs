using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;

namespace SofthemeRoomBooking.Converters
{
    public static class ApplicationUserConverter
    {
        public static ApplicationUser ToApplicationUser(this RegisterViewModel model)
        {
            return new ApplicationUser
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Email
            };
        }

        public static LayoutUserViewModel ToLayoutUserViewModel(this ApplicationUser model)
        {
            return new LayoutUserViewModel
            {
                Id = model.Id,
                UserName = $"{model.Name} {model.Surname}"
            };
        }

        public static ProfileUserViewModel ToProfileUserViewModel(this ApplicationUser model, ApplicationUserManager manager)
        {
            return new ProfileUserViewModel
            {
                Id = model.Id,
                UserName = $"{model.Name} {model.Surname}",
                Email = model.Email,
                AdminRole = manager.GetRoles(model.Id).Count != 0
            };
        }

        public static EditUserViewModel ToEditUserViewModel(this ApplicationUser model, ApplicationUserManager manager)
        {
            return new EditUserViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                AdminRole = manager.GetRoles(model.Id).Count != 0
            };
        }

        public static ApplicationUser ToApplicationUser(this EditUserViewModel model, ApplicationUser user)
        {
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;

            return user;
        }
    }
}