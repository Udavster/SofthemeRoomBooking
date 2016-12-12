using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Converters
{
    public static class RoomEquipmentViewModelConverter
    {
        public static RoomEquipmentViewModel ToRoomEquipmentViewModel(this RoomEquipmentModel model)
        {
           return new RoomEquipmentViewModel()
            {
                Id = model.Id,
                Equipments = model.Equipments,
                Equipment = model.Equipment,
                IsAvalaible = model.IsAvalaible,
                Name = model.Name
            };
        }

        public static RoomEquipmentModel ToRoomEquipmentModel(this RoomEquipmentViewModel model)
        {
            return new RoomEquipmentModel()
            {
                Id = model.Id,
                Equipments = model.Equipments,
                Equipment = model.Equipment,
                IsAvalaible = model.IsAvalaible,
                Name = model.Name
            };
        }
    }
}