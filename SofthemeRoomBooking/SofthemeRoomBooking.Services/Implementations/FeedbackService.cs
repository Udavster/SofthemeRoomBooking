using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Implementations
{
    public class FeedbackService:IFeedbackService
    {
        private SofhemeRoomBookingContext _context;
        private INotificationService _notificationService;

        public FeedbackService(SofhemeRoomBookingContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public void Save(FeedbackModel model)
        {
            var dbModel = new SofthemeRoomBooking.DAL.Feedback()
            {
                Email = model.Email,
                Message = model.Message,
                Name = model.Name,
                Surname = model.Surname,
                Created = model.Created
            };
            _context.Feedback.Add(dbModel);
            _context.SaveChanges();

        }

    }
}
