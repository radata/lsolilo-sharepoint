using System;
using FPS.Evaluation.Core;
using LS.Holiday.Core;
using Microsoft.SharePoint;

namespace LS.Holiday.EventReceivers.HolidayListEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class HolidayListEventReceiver : SPItemEventReceiver
    {
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);

            string startString = properties.AfterProperties[HolidaysFields.StartDate.Name] as string;
            string endString = properties.AfterProperties[HolidaysFields.StartDate.Name] as string;

            var author = properties.Web.CurrentUser;
            DateTime? start = DateTime.Parse(startString);
            DateTime? end = DateTime.Parse(endString);
            string title = string.Format("{0} ({1:d} - {2:d})", author.Name, start, end);

            properties.AfterProperties[SPBuiltInFieldNames.Title] = title;
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            SPFieldUserValue author = new SPFieldUserValue(properties.Web, properties.ListItem[SPBuiltInFieldId.Author].ToString());
            DateTime? start = properties.ListItem[HolidaysFields.StartDate.Name] as DateTime?;
            DateTime? end = properties.ListItem[HolidaysFields.EndDate.Name] as DateTime?;
            string title = string.Format("{0} ({1:d} - {2:d})", author.User.Name, start, end);
            properties.ListItem[SPBuiltInFieldId.Title] = title;
            properties.ListItem.Update();
        }
    }
}
