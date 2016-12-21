using SofthemeRoomBooking.DAL;
using System.Collections.Generic;
using SofthemeRoomBooking.Services.Models;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface INotificationService
    {
        void CancelEventNotification(List<string> emails, Events eventInfo, string roomName);
        void FeedbackNotification(List<string> emails, FeedbackModel model);
        void EventUserAddedNotification(string authorEmail, string subscriberEmail, Events eventInfo);
    }
}
