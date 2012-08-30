using System;
using FPS.Diagnostics;
using Microsoft.SharePoint;

namespace FPS.Core
{
    public static class SPWebHelper
    {
        /// <summary>
        /// Safes the ensure user.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="loginName">Name of the login.</param>
        /// <returns>SPUser object.</returns>
        public static SPUser SafeEnsureUser(this SPWeb web, string loginName)
        {
            SPUser res = null;
            if (!web.AllowUnsafeUpdates)
            {
                bool oldAllowUnsafeUpdate = web.AllowUnsafeUpdates;
                try
                {
                    web.AllowUnsafeUpdates = true;
                    res = web.EnsureUser(loginName);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(ex, LogType.Error);
                }
                finally
                {
                    web.AllowUnsafeUpdates = oldAllowUnsafeUpdate;
                }
            }
            else
            {
                try
                {
                    res = web.EnsureUser(loginName);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(ex, LogType.Error);
                }
            }

            return res;
        }

        /// <summary>
        /// Ensures the user by lookup value.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="userValue">The user value.</param>
        /// <returns>SPUser object.</returns>
        public static SPUser EnsureUserByLookupValue(this SPWeb web, string userValue)
        {
            if (userValue.IsNullOrEmpty())
                return null;

            var user = new SPFieldUserValue(web, userValue);
            return user.LookupId == -1 ? web.EnsureUser(user.LookupValue) : user.User;
        }
    }
}
