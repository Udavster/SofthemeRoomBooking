﻿namespace SofthemeRoomBooking.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public bool AdminRole { get; set; }
    }
}