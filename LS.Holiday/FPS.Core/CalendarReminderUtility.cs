using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace FPS.Core
{
    /// <summary>
    /// Base class for handling calendar items.
    /// </summary>        
    public class CalendarReminderUtility
    {
        #region Enums

        /// <summary>
        /// Defines the message type in CreateMessage method.
        /// </summary>
        private enum MessageType
        {
            Meeting,
            ToDo
        }

        #endregion

        #region Fields

        private const string ContentTypeCalendar = "text/calendar";

        // TODO: move to resources in future
        private const string MeetingRequestBodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\"><HTML><HEAD><META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\"><META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\"><TITLE>{0}</TITLE></HEAD><BODY><!-- Converted from text/plain format --><P><FONT SIZE=2>Type:Single Meeting<BR>Organizer:{1}<BR>Start Time:{2}<BR>End Time:{3}<BR>Time Zone:{4}<BR>Location:{5}<BR><BR>*~*~*~*~*~*~*~*~*~*<BR><BR>{6}<BR></FONT></P>{7}</BODY></HTML>";
        private const string ToDoItemBodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\"><HTML><HEAD><META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\"><META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\"><TITLE>{0}</TITLE></HEAD><BODY><!-- Converted from text/plain format --><P><FONT SIZE=2>Type:Task Reminder<BR>Organizer:{1}<BR>Time:{2}<BR>Time Zone:{3}<BR><BR>*~*~*~*~*~*~*~*~*~*<BR><BR><h3>Lista {4} zawiera zadania wymagające Twojej uwagi</h3><div>{5}</div><BR></FONT></P>{6}</BODY></HTML>";
        private const string CalendarNameMeeting = "meeting.ics";
        private const string CalendarNameToDo = "todo.ics";

        #endregion

        #region Methods

        /// <summary>
        /// Creates a MailMessage based on the item parameter
        /// and sends it as a meeting-formatted email to all attendies.
        /// </summary>
        /// <param name="web">Web with a configure SMTP server.</param>
        /// <param name="item">Meeting parameters.</param>
        public static void SendCalendarMeetingItem(SPWeb web, CalendarItem item)
        {
            if (item == null)
                throw new NullReferenceException("Create the item first!");

            var message = CreateEventMessage(item, MessageType.Meeting);
            Utilities.SendEmail(web, message);
        }

        /// <summary>
        /// Creates a MailMessage based on the item parameter
        /// and sends it as a todo-formated email to the Recipient.
        /// </summary>
        /// <param name="web">Web with a configure SMTP server.</param>
        /// <param name="item">ToDo item parameters.</param>
        public static void SendCalendarToDoItem(SPWeb web, CalendarItem item)
        {
            if (item == null)
                throw new NullReferenceException("Create the item first!");

            var message = CreateEventMessage(item, MessageType.ToDo);
            Utilities.SendEmail(web, message);
        }

        private static MailMessage CreateEventMessage(CalendarItem calendarItem, MessageType messageType)
        {
            var calendarName = string.Empty;
            var bodyText = string.Empty;
            var bodyHTML = string.Empty;
            var message = new MailMessage();

            switch (messageType)
            {
                case MessageType.Meeting:
                    calendarName = CalendarNameMeeting;

                    // Create message body parts
                    // Create the Body in text format
                    bodyText = string.Format(
                        "Type:Single Meeting\r\nOrganizer: {0}\r\nStart Time:{1}\r\nEnd Time:{2}\r\nTime Zone:{3}\r\nLocation: {4}\r\n\r\n*~*~*~*~*~*~*~*~*~*\r\n\r\n{5}",
                        calendarItem.OrganizerName,
                        calendarItem.StartInLongFormat,
                        calendarItem.EndInLongFormat,
                        TimeZone.CurrentTimeZone.StandardName,
                        calendarItem.Location,
                        calendarItem.Summary);

                    // Create the Body in HTML format
                    bodyHTML = string.Format(
                        MeetingRequestBodyHTML,
                        SPHttpUtility.HtmlEncode(calendarItem.Title),
                        SPHttpUtility.HtmlEncode(calendarItem.OrganizerName),
                        SPHttpUtility.HtmlEncode(calendarItem.StartInLongFormat),
                        SPHttpUtility.HtmlEncode(calendarItem.EndInLongFormat),
                        SPHttpUtility.HtmlEncode(TimeZone.CurrentTimeZone.StandardName),
                        SPHttpUtility.HtmlEncode(calendarItem.Location),
                        SPHttpUtility.HtmlEncode(calendarItem.Summary),
                        Properties.Resources.EmailFooterFPS);

                    calendarItem.Attendees.ForEach(attendee => message.To.Add(attendee));
                    break;
                case MessageType.ToDo:
                    calendarName = CalendarNameToDo;

                    // Create message body parts
                    // Create the Body in text format
                    bodyText = string.Format(
                        "Title: {0}\r\nType:Task reminder\r\nOrganizer: {1}\r\nTime:{2}\r\nTime Zone:{3}\r\n\r\n*~*~*~*~*~*~*~*~*~*\r\n\r\n{4}",
                        calendarItem.Title,
                        calendarItem.OrganizerName,
                        calendarItem.StartInLongFormat,
                        TimeZone.CurrentTimeZone.StandardName,
                        calendarItem.Summary);

                    // Create the Body in HTML format
                    bodyHTML = string.Format(
                        ToDoItemBodyHTML,
                        SPHttpUtility.HtmlEncode(calendarItem.Title),
                        SPHttpUtility.HtmlEncode(calendarItem.OrganizerName),
                        SPHttpUtility.HtmlEncode(calendarItem.StartInLongFormat),
                        SPHttpUtility.HtmlEncode(TimeZone.CurrentTimeZone.StandardName),
                        SPHttpUtility.HtmlEncode(calendarItem.Summary),
                        calendarItem.HtmlDescription,
                        Properties.Resources.EmailFooterFPS);

                    message.To.Add(calendarItem.Recipient);
                    break;
                default:
                    return null;
            }

            // Set up the different mime types contained in the message
            var textType = new ContentType(MediaTypeNames.Text.Plain);
            var htmlType = new ContentType(MediaTypeNames.Text.Html);
            var calendarType = new ContentType(ContentTypeCalendar);

            // Add parameters to the calendar header
            calendarType.Parameters.Add("method", calendarItem.IsCancelled ? "CANCEL" : "REQUEST");
            calendarType.Parameters.Add("name", calendarName);

            var textView = AlternateView.CreateAlternateViewFromString(bodyText, textType);
            message.AlternateViews.Add(textView);

            var htmlView = AlternateView.CreateAlternateViewFromString(bodyHTML, htmlType);
            message.AlternateViews.Add(htmlView);

            var calendarView = AlternateView.CreateAlternateViewFromString(CreateVCalendarItem(calendarItem, messageType), calendarType);
            calendarView.TransferEncoding = TransferEncoding.SevenBit;
            message.AlternateViews.Add(calendarView);

            message.From = new MailAddress(calendarItem.OrganizerMail);
            message.Subject = calendarItem.Title;

            return message;
        }

        /// <summary>
        /// Creates a VCALENDAR event item as a properly formatted string with parameters
        /// set according to the calendarItem parameter.
        /// </summary>
        /// <param name="calendarItem">Object containing parameters of the event.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>A string formated as a VCALENDAR email section.</returns>
        private static string CreateVCalendarItem(CalendarItem calendarItem, MessageType messageType)
        {
            var calendarBuilder = new StringBuilder();
            calendarBuilder.AppendLine("BEGIN:VCALENDAR");
            calendarBuilder.AppendLine("VERSION:2.0");
            calendarBuilder.AppendLine(string.Format("METHOD:{0}", calendarItem.IsCancelled ? "CANCEL" : "REQUEST"));
            calendarBuilder.AppendLine("BEGIN:VEVENT");
            calendarBuilder.AppendLine("CLASS:PUBLIC");
            calendarBuilder.AppendLine(string.Format("CREATED:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            calendarBuilder.AppendLine(string.Format("DESCRIPTION:{0}", calendarItem.Title));
            calendarBuilder.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", calendarItem.Start));
            calendarBuilder.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", calendarItem.End));
            calendarBuilder.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            calendarBuilder.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":mailto:{1}", calendarItem.OrganizerName, calendarItem.OrganizerMail));
            calendarBuilder.AppendLine(string.Format("LAST-MODIFIED:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            calendarBuilder.AppendLine(string.Format("UID:{0}", calendarItem.Id)); // DateTime.Now));
            calendarBuilder.AppendLine(string.Format("LOCATION:{0}", calendarItem.Location));
            calendarBuilder.AppendLine(string.Format("SUMMARY;LANGUAGE=en-us:{0}", calendarItem.Summary));

            if (messageType == MessageType.Meeting)
                calendarBuilder.AppendLine(string.Format("ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{0}\":MAILTO:{0}", calendarItem.Attendees));
            else
                calendarBuilder.AppendLine(string.Format("ATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{0}\":MAILTO:{0}", calendarItem.Recipient));

            calendarBuilder.AppendLine(string.Format("STATUS:{0}", calendarItem.IsCancelled ? "CANCELLED" : "CONFIRMED"));
            calendarBuilder.AppendLine("SEQUENCE:0");
            calendarBuilder.AppendLine("BEGIN:VALARM");
            calendarBuilder.AppendLine("TRIGGER:-PT720M");
            calendarBuilder.AppendLine("ACTION:DISPLAY");
            calendarBuilder.AppendLine("DESCRIPTION:Reminder");
            calendarBuilder.AppendLine("END:VALARM");
            calendarBuilder.AppendLine("END:VEVENT");
            calendarBuilder.AppendLine("END:VCALENDAR");

            return calendarBuilder.ToString();
        }

        #endregion
    }
}
