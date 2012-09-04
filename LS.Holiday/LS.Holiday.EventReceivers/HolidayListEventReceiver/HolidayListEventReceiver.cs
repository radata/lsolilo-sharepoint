using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace LS.Holiday.EventReceivers.HolidayListEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class HolidayListEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            SPFieldUserValue author = new SPFieldUserValue(properties.Web, properties.ListItem[SPBuiltInFieldId.Author].ToString());
            DateTime? start = properties.ListItem["StartDate"] as DateTime?;
            DateTime? end = properties.ListItem["EndDate"] as DateTime?;
            string title = string.Format("{0} ({1:d} - {2:d})", author.User.Name, start, end);
            properties.ListItem[SPBuiltInFieldId.Title] = title;
            properties.ListItem.Update();


        }
    }
}
