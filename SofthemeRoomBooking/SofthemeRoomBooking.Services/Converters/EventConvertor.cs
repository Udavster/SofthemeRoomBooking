using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Converters
{
    public static class EventConvertor
    {
        private static readonly string[] _month =
        {
            "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября",
            "Октября", "Ноября", "Декабря"
        };

        private static readonly string[] _dayOfWeek = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Cб" };

        public static Events ToEventEntity(this EventModel model, string userId)
        {
            return new Events
            {
                Title = model.Title,
                Description = model.Description,
                Id_room = model.IdRoom,
                Nickname = model.Nickname,
                Publicity = model.Publicity,
                AllowRegistration = model.AllowRegistration,
                Id_user = userId,
                Start = model.StartTime,
                Finish = model.FinishTime,
            };
        }

        public static EventModel ToEventModel(this Events model)
        {
            return new EventModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Nickname = model.Nickname,
                Publicity = model.Publicity,
                AllowRegistration = model.AllowRegistration,
                IdRoom = model.Id_room,
                StartTime = model.Start,
                FinishTime = model.Finish
            };
        }

        public static EventDetailsModel ToEventDetailsModel(this Events model, int participantsQuantity)
        {
            return new EventDetailsModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Nickname = model.Nickname,
                DayOfWeek = _dayOfWeek[(int)model.Start.DayOfWeek],
                Month = _month[model.Start.Month - 1],
                StartTime = model.Start,
                FinishTime = model.Finish,
                ParticipantsQuantity = participantsQuantity,
                UserId = model.Id_user,
                Publicity = model.Publicity,
                AllowRegistration = model.AllowRegistration
            };
        }

        public static EventWeekModel ToEvent(this Events events)
        {
            return new EventWeekModel
            {
                Id = events.Id,
                Title = events.Title.Length > 20 ? events.Title.Substring(0,20) + "..." : events.Title,
                Description = events.Description,
                Publicity = events.Publicity,
                Start = events.Start.ToString("yyyy-MM-dd HH:mm"),
                Finish = events.Finish.ToString("yyyy-MM-dd HH:mm")
            };
        }

        public static EventWeekModel ToPrivateEvent(this Events events)
        {
            return new EventWeekModel
            {
                Id = events.Id,
                Publicity = events.Publicity,
                Description = "",
                Title = "",
                Start = events.Start.ToString("yyyy-MM-dd HH:mm"),
                Finish = events.Finish.ToString("yyyy-MM-dd HH:mm")
            };
        }
    }
}
