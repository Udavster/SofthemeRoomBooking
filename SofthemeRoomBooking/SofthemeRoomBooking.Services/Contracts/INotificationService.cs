using SofthemeRoomBooking.DAL;
using System.Collections.Generic;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface INotificationService
    {
        void CancelEventNotification(List<string> emails, Events eventInfo);
        void FeedbackNotification(List<string> emails, FeedbackModel model);
    }
}
