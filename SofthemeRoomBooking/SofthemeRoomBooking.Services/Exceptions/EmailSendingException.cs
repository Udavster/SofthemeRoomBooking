using System;

namespace SofthemeRoomBooking.Services.Exceptions
{
    public class EmailSendingException:Exception
    {
        public EmailSendingException(string message):base(message)
        {
        }

        public EmailSendingException(string message, Exception innerException):base(message,innerException)
        {
        }

    }
}
