using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Utilities;

namespace FPS.Diagnostics
{
    internal static class DiagnosticsResourceHelper
    {
        #region Fields

        private static string _resourceName = "$Resources:FPS.Diagnostics.Strings,{0}";
        private static string _defaultResource = "FPS.Diagnostics.Strings";
        private static uint _defaultLanguageCode = 1033;

        #endregion

        #region Properties

        /// <summary>
        /// Gets Name of the default event source.
        /// </summary>
        public static string DefaultEventSource
        {
            get { return SPUtility.GetLocalizedString(string.Format(_resourceName, "DefaultEventSource"), _defaultResource, _defaultLanguageCode); }
        }

        /// <summary>
        /// Gets Diagnostics lists site url.
        /// </summary>
        public static string DiagnosticsListsSiteUrl
        {
            get { return SPUtility.GetLocalizedString(string.Format(_resourceName, "DiagnosticsListsSiteUrl"), _defaultResource, _defaultLanguageCode); }
        }

        /// <summary>
        /// Gets Name of the Event Log list.
        /// </summary>
        public static string EventLogListName
        {
            get { return SPUtility.GetLocalizedString(string.Format(_resourceName, "EventLogListName"), _defaultResource, _defaultLanguageCode); }
        }

        /// <summary>
        /// Gets Name of the system event log.
        /// </summary>
        public static string EventLogName
        {
            get { return SPUtility.GetLocalizedString(string.Format(_resourceName, "EventLogName"), _defaultResource, _defaultLanguageCode); }
        }

        #endregion
    }
}
