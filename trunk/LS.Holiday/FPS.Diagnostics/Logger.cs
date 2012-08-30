using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace FPS.Diagnostics
{
    public sealed class Logger : ILogger
    {
        #region Fields

        private static Logger instance;

        private List<ILoggerTarget> _targets;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                    instance = new Logger();

                return instance;
            }
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        public List<ILoggerTarget> Targets
        {
            get { return _targets; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created.
        /// </summary>
        private Logger()
        {
            Configure();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs the specified message to EventLog.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        public void Log(LogEntry logEntry)
        {
            try
            {
                // fill the missing data
                logEntry.EventTime = DateTime.Now;
                if (SPContext.Current != null)
                {
                    logEntry.HostName = SPContext.Current.Site.HostName;
                    logEntry.CurrentUserLogin = SPContext.Current.Web.CurrentUser.LoginName;
                }
                else
                    logEntry.HostName = Environment.MachineName;

                // write event to log for every configured target
                if (logEntry.Type != LogType.Internal)
                {
                    foreach (var target in _targets)
                        target.WriteLogEntry(logEntry.Clone());
                }
                else
                {
                    // if event is an "internal" type - log it only with system event logger
                    var eventLogger = _targets.Where(target => target.GetType() == typeof(EventLoggerTarget)).FirstOrDefault();

                    if (eventLogger != null)
                        eventLogger.WriteLogEntry(logEntry.Clone());
                }
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Logs the specified message to EventLog.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <param name="logType">Type of the log.</param>
        public void Log(string message, string source, LogType logType = LogType.Information)
        {
            // create the log entry object
            var logEntry = new LogEntry
            {
                Message = message,
                Source = source ?? DiagnosticsResourceHelper.DefaultEventSource,
                Type = logType
            };

            Log(logEntry);
        }

        /// <summary>
        /// Logs the specified messageto EventLog.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source type.</param>
        /// <param name="logType">Type of the log.</param>
        public void Log(string message, SourceType source, LogType logType = LogType.Information)
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Source = source.ToString(),
                Type = logType
            };

            Log(logEntry);
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logType">Type of the log.</param>
        public void Log(Exception exception, LogType logType = LogType.Error)
        {
            // create log entry
            var logEntry = new LogEntry(exception);
            logEntry.Type = logType;

            Log(logEntry);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public void Log(string message, LogType logType)
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Type = logType
            };

            Log(logEntry);
        }

        /// <summary>
        /// Logs an information.
        /// </summary>
        /// <param name="format">String format.</param>
        /// <param name="args">Arguments for string.Format.</param>
        public void Information(string format, params object[] args)
        {
            Log(string.Format(format, args: args), LogType.Information);
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="format">String format.</param>
        /// <param name="args">Arguments for string.Format.</param>
        public void Warning(string format, params object[] args)
        {
            Log(string.Format(format, args: args), LogType.Warning);
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="format">String format.</param>
        /// <param name="args">Arguments for string.Format.</param>
        public void Error(string format, params object[] args)
        {
            Log(string.Format(format, args: args), LogType.Error);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Configures this instance.
        /// </summary>
        private void Configure()
        {
            // TODO: [TJ]: at the moment the targets are not configured by an external source (like a config file) - the configuration is hardcoded here, should be changed in the future
            _targets = new List<ILoggerTarget>();

            // add event logger target
            var eventLoggerTarget = new EventLoggerTarget();
            eventLoggerTarget.EventLogName = DiagnosticsResourceHelper.EventLogName;
            _targets.Add(eventLoggerTarget);

            // add sharepoint list target
            var spListTarget = new SPListLoggerTarget();
            spListTarget.ListName = DiagnosticsResourceHelper.EventLogListName;
            spListTarget.SiteUrl = DiagnosticsResourceHelper.DiagnosticsListsSiteUrl;
            _targets.Add(spListTarget);
        }

        #endregion
    }
}
