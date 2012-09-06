using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace LS.Holiday.Core
{
    public static class EmailHelper
    {
        /// <summary>
        /// Gets the standard headers.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="addressTo">The address to.</param>
        /// <param name="addressFrom">The address from.</param>
        /// <returns>Standard headers.</returns>
        public static StringDictionary GetStandardHeaders(string subject, string addressTo, string addressFrom)
        {
            StringDictionary headers = new StringDictionary();
            headers.Add("to", addressTo);
            headers.Add("from", addressFrom);
            headers.Add("subject", subject);
            headers.Add("MIME-Version", "1.0");
            headers.Add("Content-Type", "text/html; charset=UTF-8");
            return headers;
        }

        /// <summary>
        /// Displays the html tagged link to task.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="taskId">The task id.</param>
        /// <returns>
        /// The html tagged link.
        /// </returns>
        public static string GenerateTaskLink(SPWeb web, int taskId, string title)
        {
            var result = string.Format("<a href='{0}/Lists/Tasks/EditForm.aspx?ID={1}&IsDlg=1'>{2}</a>", web.Url, taskId, title);
            return result;
        }

        /// <summary>
        /// Generates the holiday link.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="itemId">The item id.</param>
        /// <returns></returns>
        public static string GenerateHolidayLink(SPWeb web, int itemId, string title)
        {
            return string.Format("<a href='{0}/Lists/Holidays/DispForm.aspx?ID={1}&IsDlg=1'>{2}</a>", web.Url, itemId, title);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="content">The content.</param>
        public static void SendEmail(SPWeb web, string from, string to, string subject, string content)
        {
            StringDictionary headers = GetStandardHeaders(subject, to, from);
            SPUtility.SendEmail(web, headers, content); ;
        }
    }
}
