﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Models
{
    public class EventViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Заголовок не может быть длиннее 50 символов")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Дополнительная информация не может быть длиннее 500 символов")]
        public string Description { get; set; }

        [Required]
        public bool Private { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Неверный идентификатор комнаты")]
        public int IdRoom { get; set; }

        [StringLength(50, ErrorMessage = "Имя организатора не может быть длиннее 50 символов")]
        public string Nickname { get; set; }

        public IEnumerable<SelectListItem> Rooms { get; set; }

        [Required]
        [Range(1, 31, ErrorMessage = "День должен находиться в интервале [1, 31]")]
        public int Day { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Месяц должен находиться в интервале [1, 12]")]
        public int Month { get; set; }

        [Required]
        [Range(2016, int.MaxValue, ErrorMessage = "Год не может быть меньше {0}")]
        public int Year { get; set; }

        [Required]
        [Range(0, 23, ErrorMessage = "Час не может выходить за интервал [0, 23]")]
        public int StartHour { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "Минуты не могут выходить за интервал [0, 59]")]
        public int StartMinutes { get; set; }

        [Required]
        [Range(0, 23, ErrorMessage = "Час не может выходить за интервал [0, 23]")]
        public int EndHour { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "Минуты не могут выходить за интервал [0, 59]")]
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