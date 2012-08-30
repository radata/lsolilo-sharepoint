using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using FPS.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace FPS.Core
{
    public static class Utilities
    {
        #region Private fields

        private static readonly string _appendFieldHeader = "<br />{0} ({1:yyyy-MM-dd HH:mm}): ";

        #endregion

        #region Methods

        /// <summary>
        /// Gets the UTC date time.
        /// </summary>
        /// <param name="dateObject">The date object.</param>
        /// <param name="web">The web.</param>
        /// <returns>DateTime object.</returns>
        public static DateTime? GetUtcDateTime(object dateObject, SPWeb web)
        {
            var castedObject = dateObject as string;
            if (castedObject == null)
                return null;

            DateTime date;
            if (DateTime.TryParse(castedObject, out date))
            {
                date = web.RegionalSettings.TimeZone.LocalTimeToUTC(date);
                return date.ToUniversalTime();
            }

            return null;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="usersObject">The users object.</param>
        /// <param name="web">The web.</param>
        /// <returns>Users list.</returns>
        public static List<SPUser> GetUsers(object usersObject, SPWeb web)
        {
            var users = new List<SPUser>();
            var castedObject = usersObject as string;
            if (castedObject != null)
            {
                var splitUsers = castedObject.Split(SPConstants.LookupFieldSplitter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string splitUser in splitUsers)
                {
                    int id = 0;
                    if (int.TryParse(splitUser, out id))
                    {
                        var user = web.SiteUsers.GetByID(id);
                        if (user != null)
                            users.Add(user);
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Gets the versioned multi line text as HTML.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>The text as HTML.</returns>
        public static string GetVersionedMultiLineTextAsHTML(SPListItem item, string displayName)
        {
            var stringBuilder = new StringBuilder();
            foreach (SPListItemVersion version in item.Versions)
            {
                var field = version.Fields[displayName] as SPFieldMultiLineText;
                if (field != null)
                {
                    string comment = field.GetFieldValueAsHtml(version[displayName]);
                    if (!comment.IsNullOrEmpty() && comment != "<div></div>")
                    {
                        if (version.CreatedBy.User.LoginName != item.Web.Site.SystemAccount.LoginName)
                            stringBuilder.AppendFormat(_appendFieldHeader, version.CreatedBy.User.Name, version.Created.ToLocalTime());

                        stringBuilder.Append(comment);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats the entry as if it would be created by System Account.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="entry">The entry.</param>
        /// <returns>Entry as created by System Account.</returns>
        public static string CreateSystemAccountEntry(SPWeb web, string entry)
        {
            var result = entry;

            try
            {
                var systemAccount = web.SafeEnsureUser(web.Site.SystemAccount.LoginName);
                var header = string.Format(_appendFieldHeader, systemAccount.Name, DateTime.Now.ToLocalTime());
                result = string.Format("{0} {1}", header, entry);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex);
            }

            return result;
        }

        /// <summary>
        /// Appends text as a new line to multi line HTML text.
        /// </summary>
        /// <param name="sourceText">The source text.</param>
        /// <param name="newLine">The new line.</param>
        /// <returns>The combined HTML-formatted text.</returns>
        public static string AppendLineBeforeMultiLineHTMLText(string sourceText, string newLine)
        {
            return newLine + sourceText;
        }

        /// <summary>
        /// Verify and if necessary modify the task list to ensure that the required fields are available.
        /// </summary>
        /// <param name="list">The list to be verified.</param>
        /// <param name="contentType">The content type id to add.</param>
        public static void VerifyModifyTaskList(SPList list, string contentType)
        {
            var contentTypeId = new SPContentTypeId(contentType);

            list.ContentTypesEnabled = true;
            var matchContentTypeId = list.ContentTypes.BestMatch(contentTypeId);

            if (matchContentTypeId.Parent.CompareTo(contentTypeId) != 0)
            {
                var availableContentType = list.ParentWeb.AvailableContentTypes[contentTypeId];
                list.ContentTypes.Add(availableContentType);
                list.Update();
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="message">The message.</param>
        public static void SendEmail(SPWeb web, MailMessage message)
        {
            if (!SPUtility.IsEmailServerSet(web))
                throw new SPException("SMTP Server is not configured.");

            var server = web.Site.WebApplication.OutboundMailServiceInstance.Server.Address;
            SendEmail(server, message);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="message">The message.</param>
        public static void SendEmail(string server, MailMessage message)
        {
            var client = new SmtpClient(server);
            try
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new SPException(string.Format("There was a problem sending email message to: {0}", message.To), ex);
            }
        }

        #endregion
    }
}
