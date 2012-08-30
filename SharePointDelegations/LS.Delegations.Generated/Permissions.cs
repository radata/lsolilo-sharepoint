using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace LS.Delegations.Generated
{
    public class Permissions
    {
        #region Public Methods
        /// <summary>
        /// Grants the permission to specified user or group.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleType">Type of the role.</param>
        public static void GrantPermission(SPWeb web, SPSecurableObject subject, string userName, SPRoleType roleType)
        {
            SPPrincipal user = TryGetUser(userName, web);
            SPPrincipal principal = user != null ? user : (SPPrincipal)web.SiteGroups[userName];
            SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(roleType);
            SPRoleAssignment roleAssignment = new SPRoleAssignment(principal);
            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
            subject.RoleAssignments.Add(roleAssignment);
        }

        /// <summary>
        /// Revokes the premission from specified user or group.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="userName">Name of the user.</param>
        public static void RevokePremission(SPWeb web, SPSecurableObject subject, string userName)
        {
            SPPrincipal user = TryGetUser(userName, web);
            SPPrincipal principal = user != null ? user : (SPPrincipal)web.SiteGroups[userName];
            subject.RoleAssignments.Remove(principal);
        }
        #endregion

        #region Private Methods
        private static SPPrincipal TryGetUser(string name, SPWeb web)
        {
            return web.AllUsers.Cast<SPUser>().FirstOrDefault(u => u.Name == name);
        }
        #endregion
    }
}
