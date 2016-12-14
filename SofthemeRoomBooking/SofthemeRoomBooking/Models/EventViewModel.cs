using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Название не может быть длиннее 50 символов")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool Private { get; set; }

        public int IdRoom { get; set; }

        public string Nickname { get; set; }

        public IEnumerable<SelectListItem> Rooms { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int StartHour { get; set; }

        public int StartMinutes { get; set; }

        public int EndHour { get; set; }

        public int EndMinutes { get; set; }


        public EventViewModel() { }

        public EventViewModel(RoomModel[] rooms)
        {
            SetUnlockedRooms(rooms);
        }

        public void SetUnlockedRooms(RoomModel[] rooms)
        {
            Rooms = rooms.Select(room => new SelectListItem
            {
                Text = room.Name,
                Value = room.Id.ToString(),
                Selected = "select" == room.Id.ToString()
            });
        }
    }
}