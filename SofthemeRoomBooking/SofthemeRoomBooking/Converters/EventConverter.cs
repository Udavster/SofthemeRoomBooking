using System;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.EventViewModel;
using SofthemeRoomBooking.Services.Models;
using SofthemeRoomBooking.Services.Models.EventModel;

namespace SofthemeRoomBooking.Converters
{
    public static class EventConverter
    {
        public static EventViewModel ToEventViewModel(this EventModel model, RoomModel[] rooms)
        {
            return new EventViewModel(rooms)
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Private = !model.Publicity,
                AllowRegistration = model.AllowRegistration,
                IdRoom = model.IdRoom,
                Nickname = model.Nickname,
                Day = model.StartTime.Day,
                Month = model.StartTime.Month,
                Year = model.StartTime.Year,
                StartHour = model.StartTime.Hour,
                StartMinutes = model.StartTime.Minute,
                EndHour = model.FinishTime.Hour,
                EndMinutes = model.FinishTime.Minute
            };
        }

        public static EventModel ToEventModel(this EventViewModel model)
        {
            return new EventModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                IdRoom = model.IdRoom,
                Nickname = model.Nickname,
                Publicity = !model.Private,
                AllowRegistration = model.AllowRegistration,
                StartTime = new DateTime(model.Year, model.Month, model.Day, model.StartHour, model.StartMinutes, 0),
                FinishTime = new DateTime(model.Year, model.Month, model.Day, model.EndHour, model.EndMinutes, 0)
            };
        }

        public static EventDetailsViewModel ToEventDetailsViewModel(this EventDetailsModel model, string name)
        {
            return new EventDetailsViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Nickname = model.Nickname,
                StartTime = model.StartTime,
                FinishTime = model.FinishTime,
                ParticipantsQuantity = model.ParticipantsQuantity,
                DayOfWeek = model.DayOfWeek,
                Month = model.Month,
                UserId = model.UserId,
                Publicity = model.Publicity,
                AllowRegistration = model.AllowRegistration,
                UserName = name

            };
        }

        public static EventIndexViewModel ToEventIndexViewModel(this EventIndexModel model, ApplicationUser organizator)
        {
            return new EventIndexViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Nickname = model.Nickname,
                Organizator = string.IsNullOrWhiteSpace(model.Nickname) ? $"{organizator.Name} {organizator.Surname}" : model.Nickname,
                Private = !model.Publicity,
                AllowRegistration = model.AllowRegistration,
                ParticipantsQuantity = model.ParticipantsQuantity,
                IdRoom = model.IdRoom,
                IdUser = model.IdUser,
                Day = model.StartTime.Day,
                Month = model.StartTime.Month,
                Year = model.StartTime.Year,
                StartHour = model.StartTime.Hour,
                StartMinutes = model.StartTime.Minute,
                EndHour = model.FinishTime.Hour,
                EndMinutes = model.FinishTime.Minute
            };
        }

        public static EventParticipantViewModel ToEventParticipantViewModel(this EventParticipantModel model)
        {
            return new EventParticipantViewModel
            {
                Id = model.Id,
                IdEvent = model.IdEvent
            };
        }

        public static EventParticipantModel ToEventParticipantModel(this EventParticipantViewModel model)
        {
            return new EventParticipantModel
            {
                IdEvent = model.IdEvent,
                Email = model.Email
            };
        }
    }
}