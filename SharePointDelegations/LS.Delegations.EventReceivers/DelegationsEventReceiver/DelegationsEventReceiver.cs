using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using LS.Delegations.Generated;

namespace LS.Delegations.EventReceivers.DelegationsEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class DelegationsEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            properties.ListItem["Title"] = string.Format("{0} ({1:d})",
                properties.ListItem[DelegationsFields.Place],
                properties.ListItem[DelegationsFields.Start]);
            properties.ListItem.Update();
        }
    }
}
