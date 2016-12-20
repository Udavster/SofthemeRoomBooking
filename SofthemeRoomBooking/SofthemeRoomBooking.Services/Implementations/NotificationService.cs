using System.Collections.Generic;
using SofthemeRoomBooking.Services.Contracts;
using SofthemeRoomBooking.Services.Models;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using System;
using SofthemeRoomBooking.DAL;
using SofthemeRoomBooking.Services.Exceptions;

namespace SofthemeRoomBooking.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly string username = ConfigurationManager.AppSettings["sendGridName"];
        private readonly string pswd = ConfigurationManager.AppSettings["sendGridPassword"];
        private string emailSender = "admin@softhemeroombooking.com";
        private string senderName = "SofthemeRoomBooking";

        private void SendMail(EmailModel model)
        {
            try
            {
                var message = new SendGridMessage();
                message.AddTo(model.Emails);
                message.From = new MailAddress(emailSender, senderName);
                message.Subject = model.Subject;
                message.Text = model.Text;

                var credentials = new NetworkCredential(username, pswd);
                var transportWeb = new Web(credentials);

                if (transportWeb != null)
                {
                    transportWeb.Deliver(message);
                }
            }
            catch (Exception ex)
            {
                throw new EmailSendingException("Message sending failed", ex);
            }
        }

        public void CancelEventNotification(List<string> emails, Events eventInfo, string roomName)
        {
            EmailModel model = new EmailModel()
            {
                Emails = emails,
                Subject = "Event cancelation",
                Text =
                           "Event " + eventInfo.Title + ", which will starts in " + eventInfo.Start.ToString("f") +
                           " in room " + roomName + " was canceled. Thanks for your attention!"

            };
            SendMail(model);
        }

        public void FeedbackNotification(List<string> emails, FeedbackModel feedback)
        {
            EmailModel model = new EmailModel()
            {
                Emails = emails,
                Subject = "Feedback from " + feedback.Name + " "+ feedback.Surname,
                Text =  feedback.Message
            };
            SendMail(model);
        }

        public void EventUserAddedNotification(string authorEmail, string subscriberEmail, Events eventInfo)
        {
            EmailModel model = new EmailModel()
            {
                Emails = new List<string>() {authorEmail},
                Subject = String.Format("New {0} subscriber", eventInfo.Title),
                Text = String.Format("Event '{0}' has new subscriber! {1} joined your event today ({2}).", 
                                     eventInfo.Title, subscriberEmail, DateTime.Now.Date)

            };
            SendMail(model);
        }

    }
}
