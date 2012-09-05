using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.Core;
using Microsoft.SharePoint;

namespace LS.Holiday.Core
{
    public static class CalendarHelper
    {
        /// <summary>
        /// Sends the calendar invitation.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="organizerName">Name of the organizer.</param>
        /// <param name="organizerAddress">The organizer address.</param>
        /// <param name="attendentAddress">The attendent address.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="title">The title.</param>
        public static void SendCalendarInvitation(SPWeb web, string organizerName, string organizerAddress, string attendentAddress,
            DateTime? start, DateTime? end, string title)
        {
            CalendarItem calendarItem = new CalendarItem()
            {
                Start = start,
                End = end,
                Attendees = new System.Net.Mail.MailAddressCollection(),
                Title = title,
                OrganizerMail = organizerAddress,
                OrganizerName = organizerName,
                Id = Guid.NewGuid().ToString(),
                Location = "no information"
            };

            calendarItem.Attendees.Add(attendentAddress);
            CalendarReminderUtility.SendCalendarMeetingItem(web, calendarItem);
        }
    }
}
