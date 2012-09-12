using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using LS.Holiday.Core;
using LS.Holiday.Core.Strings;
using FPS.Evaluation.Core;

namespace LS.Holiday.EventReceivers.TaskListEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class TaskListEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            SendLeaderNotification(properties);
        }

        /// <summary>
        /// Synchronous Before event that occurs when an existing item is changed, for example, when the user changes data in one or more fields.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPItemEventProperties"/> object that represents properties of the event handler.</param>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);

            var currentUser = properties.Web.CurrentUser;
            SPFieldUserValue authorisor = new SPFieldUserValue(properties.Web, properties.ListItem[SPBuiltInFieldId.AssignedTo].ToString());

            bool isCurrentUserAuthorisor = currentUser.LoginName == authorisor.User.LoginName;
            if (!isCurrentUserAuthorisor)
            {
                var afterProperties = properties.AfterProperties[HolidaysFields.Decision.Name];
                properties.AfterProperties[HolidaysFields.Decision.Name] = null;
                afterProperties = properties.AfterProperties[HolidaysFields.Decision.Name];
            }
        }

        /// <summary>
        /// Sends the leader notification.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void SendLeaderNotification(SPItemEventProperties properties)
        {
            string title = properties.ListItem[SPBuiltInFieldNames.Title].ToString();
            string subject = string.Format(Values.LeaderApprovalRequestMailHeader, title);
            string taskLink = EmailHelper.GenerateTaskLink(properties.Web, properties.ListItemId, title);
            string content = string.Format(Values.LeaderApprovalRequestMailContent, taskLink);
            string serviceMail = Values.HolidayServiceMailAddress;
            SPFieldUserValue manager = new SPFieldUserValue(properties.Web, properties.ListItem[SPBuiltInFieldId.AssignedTo].ToString());
            EmailHelper.SendEmail(properties.Web, serviceMail, manager.User.Email, subject, content);
        }
    }
}
