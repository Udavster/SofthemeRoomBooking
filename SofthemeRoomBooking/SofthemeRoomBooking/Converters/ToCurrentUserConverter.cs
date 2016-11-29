using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class ToCurrentUserConverter
    {
        public static CurrentUserViewModel ToCurrentUserViewModel(this LoginViewModel model)
        {
            return new CurrentUserViewModel()
            {
                Email = model.Email
            };
        }

        public static CurrentUserViewModel ToCurrentUserViewModel(this RegisterViewModel model)
        {
            return new CurrentUserViewModel()
            {
                UserName = model.Name,
                Email = model.Email
            };
        }
    }
}