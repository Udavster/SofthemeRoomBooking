using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;

namespace SofthemeRoomBooking.Converters
{
    public static class ApplicationUserConverter
    {
        public static LayoutUserViewModel ToLayoutUserViewModel(this ApplicationUser model)
        {
            return new LayoutUserViewModel { UserName = $"{model.Name} {model.Surname}" };
        }

        public static ProfileUserViewModel ToProfileUserViewModel(this ApplicationUser model)
        {
            return new ProfileUserViewModel
            {
                UserName = $"{model.Name} {model.Surname}",
                Email = model.Email
            };
        }
    }
}