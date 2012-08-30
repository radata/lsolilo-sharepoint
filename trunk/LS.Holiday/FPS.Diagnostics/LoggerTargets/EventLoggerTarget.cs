using System;
using System.Diagnostics;

namespace FPS.Diagnostics
{
    /// <summary>
    /// Writes to system event log.
    /// </summary>
    public class EventLoggerTarget : ILoggerTarget
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the event log.
        /// </summary>
        /// <value>
        /// The name of the event log.
        /// </value>
        public string EventLogName { get; set; }

        #endregion

        #region ILoggerTarget members

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="entry">Log entry.</param>
        public void WriteLogEntry(LogEntry entry)
        {
            if (entry == null)
                return;

            var eventType = EventLogEntryType.Information;

            // create category
            try
            {
                if (!EventLog.SourceExists(entry.Source))
                    EventLog.CreateEventSource(entry.Source, EventLogName);
            }
            catch
            {
                entry.Source = DiagnosticsResourceHelper.DefaultEventSource;
            }

            // get event type
            var enumAsString = entry.Type.ToString();
            if (Enum.IsDefined(typeof(EventLogEntryType), enumAsString))
                eventType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), enumAsString);

            // log the entry
            var log = new EventLog();
            log.Source = entry.Source;
            log.Log = EventLogName;
            log.WriteEntry(string.Concat(entry.Message, Environment.NewLine, entry.StackTrace), eventType);
        }

        #endregion
    }
}
