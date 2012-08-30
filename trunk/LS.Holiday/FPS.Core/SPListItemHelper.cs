using System;
using System.Collections.Generic;
using FPS.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace FPS.Core
{
    public static class SPListItemHelper
    {
        /// <summary>
        /// Sets permissions for the SPListItem.
        /// </summary>
        /// <param name="item">Permissions will be set for this item.</param>
        /// <param name="principals">Enumerable colletion of principals, who will be given permissions to the item.</param>
        /// <param name="roleType">Permission level.</param>
        /// <param name="isPermissionExclusive">If <c>true</c> all other permissions will be removed.
        /// Otherwise new permissions will be added to the existing ones.</param>
        public static void SetPermissions(this SPListItem item, IEnumerable<SPPrincipal> principals, SPRoleType roleType, bool isPermissionExclusive = true)
        {
            if (item == null)
                return;

            if (isPermissionExclusive)
                item.RemoveAllPermissions();

            foreach (SPPrincipal principal in principals)
            {
                var roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                SetPermissions(item, principal, roleDefinition, false);
            }
        }

        /// <summary>
        /// Sets permissions for the SPListItem.
        /// </summary>
        /// <param name="item">Permissions will be set for this item.</param>
        /// <param name="user">SPUser, who will be given permissions to the item.</param>
        /// <param name="roleType">Permission level.</param>
        /// <param name="isPermissionExclusive">If <c>true</c> all other permissions will be removed.
        /// Otherwise new permissions will be added to the existing ones.</param>
        public static void SetPermissions(this SPListItem item, SPUser user, SPRoleType roleType, bool isPermissionExclusive = true)
        {
            if (item == null)
                return;

            var roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
            SetPermissions(item, (SPPrincipal)user, roleDefinition, isPermissionExclusive);
        }

        /// <summary>
        /// Sets permissions for the SPListItem.
        /// </summary>
        /// <param name="item">Permissions will be set for this item.</param>
        /// <param name="principal">Principal, who will be given permissions to the item.</param>
        /// <param name="roleType">Permission level.</param>
        /// <param name="isPermissionExclusive">If <c>true</c> all other permissions will be removed.
        /// Otherwise new permissions will be added to the existing ones.</param>
        public static void SetPermissions(this SPListItem item, SPPrincipal principal, SPRoleType roleType, bool isPermissionExclusive = true)
        {
            if (item == null)
                return;

            var roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
            SetPermissions(item, principal, roleDefinition, isPermissionExclusive);
        }

        /// <summary>
        /// Sets permissions for the SPListItem.
        /// </summary>
        /// <param name="item">Permissions will be set for this item.</param>
        /// <param name="user">SPUser, who will be given permissions to the item.</param>
        /// <param name="roleDefinition">Permission level.</param>
        /// <param name="isPermissionExclusive">If <c>true</c> all other permissions will be removed.
        /// Otherwise new permissions will be added to the existing ones.</param>
        public static void SetPermissions(this SPListItem item, SPUser user, SPRoleDefinition roleDefinition, bool isPermissionExclusive = true)
        {
            if (item == null)
                return;

            SetPermissions(item, (SPPrincipal)user, roleDefinition, isPermissionExclusive);
        }

        /// <summary>
        /// Sets permissions for the SPListItem.
        /// </summary>
        /// <param name="item">Permissions will be set for this item.</param>
        /// <param name="principal">Principal, who will be given permissions to the item.</param>
        /// <param name="roleDefinition">Permission level.</param>
        /// <param name="isPermissionExclusive">If <c>true</c> all other permissions will be removed.
        /// Otherwise new permissions will be added to the existing ones.</param>
        public static void SetPermissions(this SPListItem item, SPPrincipal principal, SPRoleDefinition roleDefinition, bool isPermissionExclusive = true)
        {
            if (item == null)
                return;

            var roleAssignment = new SPRoleAssignment(principal);

            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

            if (isPermissionExclusive)
                item.RemoveAllPermissions();

            item.RoleAssignments.Add(roleAssignment);
        }

        /// <summary>
        /// Removes all existing permissions for the item.
        /// </summary>
        /// <param name="item">Permissions will be removed for this item.</param>
        public static void RemoveAllPermissions(this SPListItem item)
        {
            while (!item.RoleAssignments.IsNullOrEmpty())
                item.RoleAssignments.Remove(0);
        }

        /// <summary>
        /// Removes the user from the item's role assignment collection.
        /// </summary>
        /// <param name="item">Permissions will be removed for this item.</param>
        /// <param name="user">User to remove permissions.</param>
        public static void RemovePermissions(this SPListItem item, SPUser user)
        {
            item.RoleAssignments.Remove(user);
        }

        /// <summary>
        /// Gets the SPUser associated with the SPListItem field.
        /// </summary>
        /// <param name="item">List item.</param>
        /// <param name="userField">Field containing user data.</param>
        /// <returns><c>SPUser</c> object.</returns>
        public static SPUser GetUser(this SPListItem item, SPField userField)
        {
            var currentValueObject = item[userField.Id];
            if (currentValueObject == null)
                return null;
            var currentValue = currentValueObject.ToString();
            var field = userField as SPFieldUser;
            if (field == null)
                return null;
            var fieldValue = field.GetFieldValue(currentValue) as SPFieldUserValue;
            return fieldValue == null ? null : fieldValue.User;
        }

        /// <summary>
        /// Gets the SPUser associated with the SPListItem field.
        /// </summary>
        /// <param name="item">List item.</param>
        /// <param name="fieldGuid">Field containing user data.</param>
        /// <returns><c>SPUser</c> object.</returns>
        public static SPUser GetUser(this SPListItem item, Guid fieldGuid)
        {
            return item.GetUser(item.Fields[fieldGuid]);
        }

        /// <summary>
        /// Sets the content type of the item.
        /// </summary>
        /// <param name="item">Item to modify.</param>
        /// <param name="contentTypeId"><c>ContentType</c> to set.</param>
        public static void SetContentType(this SPListItem item, SPContentTypeId contentTypeId)
        {
            item[SPBuiltInFieldId.ContentTypeId] = item.Web.ContentTypes[contentTypeId].Id;
            item[SPBuiltInFieldId.ContentType] = item.Web.ContentTypes[contentTypeId].Name;
        }

        /// <summary>
        /// Determines whether workflow is running on the specified list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="workflowId">The workflow id.</param>
        /// <returns>
        ///   <c>true</c> if workflow is running; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWorkflowRunning(this SPListItem listItem, Guid workflowId)
        {
            foreach (SPWorkflow workflow in listItem.Workflows)
                if (workflow.ParentAssociation.BaseTemplate.Id == workflowId && (workflow.InternalState & SPWorkflowState.Running) == SPWorkflowState.Running)
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether workflow is running on the specified list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="workflowTemplateName">Name of the workflow template.</param>
        /// <returns>
        ///   <c>true</c> if workflow is running; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWorkflowRunning(this SPListItem listItem, string workflowTemplateName)
        {
            return listItem.IsWorkflowRunning(listItemWorkflowName => listItemWorkflowName == workflowTemplateName);
        }

        /// <summary>
        /// Determines whether workflow is running on the specified list item.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="workflowTemplateNameCompareMethod">The workflow template name compare method.</param>
        /// <returns>
        ///   <c>true</c> if workflow is running; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWorkflowRunning(this SPListItem listItem, Func<string, bool> workflowTemplateNameCompareMethod)
        {
            foreach (SPWorkflow workflow in listItem.Workflows)
                if (workflowTemplateNameCompareMethod(workflow.ParentAssociation.BaseTemplate.Name) && (workflow.InternalState & SPWorkflowState.Running) == SPWorkflowState.Running)
                    return true;

            return false;
        }

        /// <summary>
        /// Starts the workflow.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="workflowTemplateName">Name of the workflow template.</param>
        /// <returns>
        ///   <c>true</c> if workflow staretd successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartWorkflow(this SPListItem listItem, string workflowTemplateName)
        {
            return listItem.StartWorkflow(listItemWorkflowName => listItemWorkflowName == workflowTemplateName);
        }

        /// <summary>
        /// Starts the workflow.
        /// </summary>
        /// <param name="listItem">The list item.</param>
        /// <param name="workflowTemplateNameCompareMethod">The workflow template name compare method.</param>
        /// <returns>
        ///   <c>true</c> if workflow staretd successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartWorkflow(this SPListItem listItem, Func<string, bool> workflowTemplateNameCompareMethod)
        {
            foreach (SPWorkflowAssociation workflowAssociation in listItem.ParentList.WorkflowAssociations)
            {
                if (workflowAssociation.Enabled && workflowTemplateNameCompareMethod(workflowAssociation.BaseTemplate.Name))
                {
                    listItem.Web.Site.WorkflowManager.StartWorkflow(listItem, workflowAssociation, workflowAssociation.AssociationData, true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get field state at date.
        /// </summary>
        /// <typeparam name="T">Type of field value.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="displayName">The display name of the field.</param>
        /// <param name="date">The date.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if operation succeds. <c>False</c> otherwise.</returns>
        public static bool TryGetFieldStateAtDate<T>(this SPListItem item, string displayName, DateTime date, out T value)
        {
            if (!item.ParentList.EnableVersioning)
                throw new InvalidOperationException("Parent list does not have versioning enabled.");

            value = default(T);

            var versionValues = new Dictionary<DateTime, T>();

            foreach (SPListItemVersion version in item.Versions)
            {
                var fieldValue = version[displayName];

                if (fieldValue != null)
                    versionValues.Add(version.Created.ToLocalTime(), (T)fieldValue);
            }

            var versionDate = DateTime.MinValue;

            foreach (KeyValuePair<DateTime, T> pair in versionValues)
            {
                if (pair.Key <= date && pair.Key > versionDate)
                {
                    versionDate = pair.Key;
                    value = pair.Value;
                }
            }

            return versionValues.Count > 0 && versionDate != DateTime.MinValue;
        }

        /// <summary>
        /// Copies permissions for sourceUser to targetUser based on role types.
        /// </summary>
        /// <param name="item">This item.</param>
        /// <param name="sourceUser">The user from which to copy permissions.</param>
        /// <param name="targetUser">The user to which to copy permissions.</param>
        public static void CopyUserPermissions(this SPListItem item, SPUser sourceUser, SPUser targetUser)
        {
            if (item == null)
                return;

            SPRoleAssignment previousUserAssignment;

            try
            {
                previousUserAssignment = item.RoleAssignments.GetAssignmentByPrincipal(sourceUser);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex, LogType.Warning);
                return;
            }

            var permissionsToSet = new List<SPRoleType>();
            foreach (SPRoleDefinition roledef in previousUserAssignment.RoleDefinitionBindings)
            {
                // because SPException: "You cannot grant a user the Limited Access permission level."
                if (roledef.Type != SPRoleType.Guest)
                    permissionsToSet.Add(roledef.Type);
            }

            item.RemovePermissions(targetUser);
            foreach (var roleType in permissionsToSet)
                item.SetPermissions(targetUser, roleType, false);
        }

        /// <summary>
        /// Return false if user does not exist or has no role assignments for the given list item.
        /// </summary>
        /// <param name="item">This item.</param>
        /// <param name="user">User to verify.</param>
        /// <returns>True is there are any role assignments in this item for given user.</returns>
        public static bool AreRoleAssignmentsPresentForUser(this SPListItem item, SPUser user)
        {
            try
            {
                item.RoleAssignments.GetAssignmentByPrincipal(user);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
