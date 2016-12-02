namespace SofthemeRoomBooking.Models.UserViewModels
{
    public class ProfileUserViewModel
    {
        public string UserName { get; set; } 

        public string Email { get; set; }

        public int ActiveEvents { get; set; }

        public bool AdminRole { get; set; }
    }
}