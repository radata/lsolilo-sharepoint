using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class PermissionHelper
    {
        /// <summary>
        /// Determines whether the group is an administrator group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>
        ///   <c>true</c> if the group is an administrator group, otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAdministratorGroup(this SPGroup group)
        {
            var web = group.ParentWeb;
            return web.RoleAssignments.GetAssignmentByPrincipal(group).RoleDefinitionBindings.Contains(web.RoleDefinitions.GetByType(SPRoleType.Administrator));
        }
    }
}
