using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SofthemeRoomBooking.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class RegisterToCurrentUserConverter
    {
        public static CurrentUserViewModel Convert(this RegisterViewModel model)
        {
            return new CurrentUserViewModel()
            {
                UserName = model.UserName
            };
        }
    }
}