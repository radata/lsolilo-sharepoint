using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;

namespace LS.Holiday.Core
{
    public static class UserHelper
    {
        /// <summary>
        /// Gets the user project leader.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="serviceContext">The service context.</param>
        /// <param name="context">The context.</param>
        /// <returns>The Project Leader SPUser object.</returns>
        public static SPUser GetUserProjectLeader(SPUser user, SPServiceContext serviceContext, SPContext context)
        {
            return context.Web.AllUsers["fp\\fps_pm"];

            UserProfileManager manager = new UserProfileManager(serviceContext);
            var userProfile = manager.GetUserProfile(user.LoginName);
            var projectLeaderProfile = userProfile.GetManager();
            return context.Web.AllUsers[projectLeaderProfile.MultiloginAccounts[0]];
        }
    }
}
