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
