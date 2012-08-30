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
            DelegationApprovalCycle.RespondToDelegationAdding(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            base.ItemUpdating(properties);
            DelegationApprovalCycle.RespondToDelegationUpdating(properties);
            EventFiringEnabled = true;

        }

        /// <summary>
        /// Asynchronous After event that occurs after a new item has been added to its containing object.
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            base.ItemAdded(properties);
            DelegationApprovalCycle.RespondToItemAdded(properties);
            EventFiringEnabled = true;
        }

        /// <summary>
        /// Asynchronous After event that occurs after an existing item is changed, for example, when the user changes data in one or more fields.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPItemEventProperties"/> object that represents properties of the event handler.</param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            EventFiringEnabled = false;
            base.ItemUpdated(properties);
            DelegationApprovalCycle.RespondToItemUpdated(properties);
            EventFiringEnabled = true;
        }
    }
}
