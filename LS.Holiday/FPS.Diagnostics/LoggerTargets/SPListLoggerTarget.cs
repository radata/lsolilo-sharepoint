using System;
using System.Threading;
using Microsoft.SharePoint.Client;

namespace FPS.Diagnostics
{
    /// <summary>
    /// Writes log entries in a sharepoint list.
    /// </summary>
    public class SPListLoggerTarget : ILoggerTarget
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the list where errors are logged.
        /// </summary>
        /// <value>
        /// The name of the list.
        /// </value>
        public string ListName { get; set; }

        /// <summary>
        /// Gets or sets the site URL.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        public string SiteUrl { get; set; }

        #endregion

        #region ILoggerTarget members

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="entry">Log entry.</param>
        public void WriteLogEntry(LogEntry entry)
        {
            // if everything is set - start the log writing method in a separate thread
            if (entry != null && !string.IsNullOrEmpty(ListName) && !string.IsNullOrEmpty(SiteUrl))
                ThreadPool.QueueUserWorkItem(new WaitCallback(WriteLogEntryAsync), entry);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes the log entry - asynchronous.
        /// </summary>
        /// <param name="entry">The entry.</param>
        private void WriteLogEntryAsync(object entry)
        {
            var logEntry = (LogEntry)entry;

            try
            {
                // create the context
                using (var clientContext = new ClientContext(SiteUrl))
                {
                    // get the site
                    var site = clientContext.Web;

                    if (site == null)
                        return;

                    // get the event log list
                    var list = site.Lists.GetByTitle(ListName);
                    clientContext.Load(list);
                    clientContext.ExecuteQuery();

                    if (list == null)
                        return;

                    // add item to the list
                    var itemCreationInfo = new ListItemCreationInformation();
                    var listItem = list.AddItem(itemCreationInfo);
                    listItem[DiagnosticsFieldNames.Message] = logEntry.Message;
                    listItem[DiagnosticsFieldNames.EventTime] = logEntry.EventTime;
                    listItem[DiagnosticsFieldNames.HostName] = logEntry.HostName;
                    listItem[DiagnosticsFieldNames.Login] = logEntry.CurrentUserLogin;
                    listItem[DiagnosticsFieldNames.Source] = logEntry.Source;
                    listItem[DiagnosticsFieldNames.StackTrace] = logEntry.StackTrace;
                    listItem[DiagnosticsFieldNames.Type] = logEntry.Type.ToString();
                    listItem[DiagnosticsFieldNames.ExceptionType] = logEntry.ExceptionType;

                    listItem.Update();

                    clientContext.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex, LogType.Internal);
            }
        }

        #endregion
    }
}
