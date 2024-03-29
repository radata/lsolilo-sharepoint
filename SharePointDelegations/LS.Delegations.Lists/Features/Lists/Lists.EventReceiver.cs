using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using LS.Delegations.Generated;
using LS.Delegations.Generated;
using LS.Delegations.Lists;
using Microsoft.SharePoint;

namespace SharePointDelegations.Features.Lists
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("890382d4-2ca1-4007-87bc-e462c3dec82b")]
    public class ListsEventReceiver : SPFeatureReceiver
    {
        #region Public Methods
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            InitializeStatusList(web);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }
        #endregion

        #region Private Methods
        private static void InitializeStatusList(SPWeb web)
        {
            SPList statusList = web.Lists[DelegationsLists.DelegationStatuses.Name];

            Collection<SPListItem> items = new Collection<SPListItem>();
            foreach (SPListItem item in statusList.Items)
            {
                items.Add(item);
            }
            foreach (SPListItem item in items)
            {
                item.Delete();
            }

            foreach (DelegationApprovalStatus status in Enum.GetValues(typeof(DelegationApprovalStatus)))
            {
                SPListItem item = statusList.AddItem();
                item[SPBuiltInFieldId.Title] = EnumHelper.GetEnumDescription(status);
                item.Update();

                if (status == DelegationApprovalStatus.Approved)
                {
                    item.BreakRoleInheritance(false);
                    string groupName = string.Format("{0} {1}", web.Title, UserGroupType.Owners);
                    Permissions.GrantPermission(web, item, groupName, SPRoleType.Administrator);
                    item.Update();
                }
            }
        }
        #endregion

        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
