using System;
using LS.Delegations.Generated;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;

namespace LS.Delegations.EventReceivers
{
    public class DelegationApprovalCycle
    {
        #region Fields
        private const int Draft = 1;
        private const int ForApproval = 2;
        private const int Approved = 3;
        private static string approvalEmail = "approval@future.processing.com";
        #endregion

        #region Public Methods
        /// <summary>
        /// Responds to delegation adding.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public static void RespondToDelegationAdding(SPItemEventProperties properties)
        {
        }

        /// <summary>
        /// Responds to delegation updating.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public static void RespondToDelegationUpdating(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            RespondToDelegationUpdating(properties, web);
                        }
                    }
                });
        }

        /// <summary>
        /// Responds to delegation updating.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="web">The web.</param>
        public static void RespondToDelegationUpdating(SPItemEventProperties properties, SPWeb web)
        {
            SPFieldLookupValue statusValue = new SPFieldLookupValue(properties.AfterProperties[DelegationsFields.DelegationStatus.Name].ToString());
            string description = properties.AfterProperties[DelegationsFields.DelegationNote.Name].ToString();
            var list = web.Lists[properties.ListId];
            var listItem = list.Items.GetItemById(properties.ListItemId);

            if (statusValue.LookupId == Draft)
            {
                if (String.IsNullOrEmpty(description))
                {
                    properties.Cancel = true;
                    properties.ErrorMessage = "Description cannot be empty.";
                    properties.Status = SPEventReceiverStatus.CancelWithError;
                }
                else
                {
                    SPFieldUserValue userValue = new SPFieldUserValue(properties.Web, properties.ListItem[SPBuiltInFieldId.Author].ToString());
                    Permissions.GrantPermission(web, listItem, userValue.User.Name, SPRoleType.Contributor);
                    SendMail(web, userValue.User.Email, description);
                }
            }
            else if (statusValue.LookupId == ForApproval)
            {
                SPFieldUserValue userValue = new SPFieldUserValue(web, properties.ListItem[SPBuiltInFieldId.Author].ToString());
                Permissions.RevokePremission(web, listItem, userValue.User.Name);
                SendMail(web, approvalEmail, properties.ListItem.Url);
            }
            else if (statusValue.LookupId == Approved)
            {
                SPFieldUserValue userValue = new SPFieldUserValue(properties.Web, properties.ListItem[SPBuiltInFieldId.Author].ToString());
                Permissions.GrantPermission(web, listItem, userValue.User.Name, SPRoleType.Reader);
                SendMail(web, userValue.User.Email, properties.ListItem.Url);
            }
        }

        /// <summary>
        /// Responds to item added.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public static void RespondToItemAdded(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.RootWeb)
                        {
                            var delegations = web.Lists[DelegationsLists.Delegations.Name];
                            var item = delegations.Items.GetItemById(properties.ListItem.ID);
                            item.BreakRoleInheritance(false);
                            string groupName = string.Format("{0} {1}", web.Title, UserGroupType.Owners);
                            SPFieldUserValue userValue = new SPFieldUserValue(web, properties.ListItem[SPBuiltInFieldId.Author].ToString());
                            SPFieldLookupValue statusValue = new SPFieldLookupValue(properties.ListItem[DelegationsFields.DelegationStatus.Name].ToString());
                            Permissions.GrantPermission(web, item, groupName, SPRoleType.Contributor);

                            if (statusValue.LookupId == Draft)
                            {
                                Permissions.GrantPermission(web, item, userValue.User.Name, SPRoleType.Contributor);
                            }
                            else
                            {
                                Permissions.GrantPermission(web, item, userValue.User.Name, SPRoleType.Reader);
                                SendMail(web, approvalEmail, properties.ListItem.Url);
                            }

                            item.Update();
                        }
                    }
                });
        }

        /// <summary>
        /// Responds to item updated.
        /// </summary>
        /// <param name="properties">The properties.</param>
        public static void RespondToItemUpdated(SPItemEventProperties properties)
        {
        }
        #endregion

        #region Private Methods
        private static void GenerateTitle(SPItemEventProperties properties)
        {
            properties.AfterProperties["Title"] = string.Format("{0} ({1})",
                properties.AfterProperties["ID"],
                properties.AfterProperties[DelegationsFields.Place.Name]);
        }

        private static void SendMail(SPWeb web, string address, string content)
        {
            StringDictionary messageHeaders = new StringDictionary();
            messageHeaders.Add("to", address);
            messageHeaders.Add("subject", "Delegations Status Notification");

            SPUtility.SendEmail(web, messageHeaders, content);
        }
        #endregion
    }
}
