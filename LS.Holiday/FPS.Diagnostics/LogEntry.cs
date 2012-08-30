using System;

namespace FPS.Diagnostics
{
    /// <summary>
    /// Contains information needed to log an event.
    /// </summary>
    public class LogEntry : ICloneable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        public LogEntry()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public LogEntry(Exception exception)
        {
            if (exception != null)
            {
                // set stack trace
                var stackTrace = exception.StackTrace;

                // if inner exception exists - append it to the stack trace
                if (exception.InnerException != null)
                    stackTrace = string.Format("{0} \r\n Inner exception: {1}", stackTrace, exception.InnerException.ToString());

                Message = exception.Message;
                Source = exception.Source ?? DiagnosticsResourceHelper.DefaultEventSource;
                Type = LogType.Error;
                ExceptionType = exception.GetType().ToString();
                StackTrace = stackTrace;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the logged message.
        /// </summary>
        /// <value>
        /// The logged message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the logging source.
        /// </summary>
        /// <value>
        /// The logging source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the log type.
        /// </summary>
        /// <value>
        /// The log type.
        /// </value>
        public LogType Type { get; set; }

        /// <summary>
        /// Gets or sets the current user login.
        /// </summary>
        /// <value>
        /// The current user login.
        /// </value>
        public string CurrentUserLogin { get; set; }

        /// <summary>
        /// Gets or sets the event time.
        /// </summary>
        /// <value>
        /// The event time.
        /// </value>
        public DateTime? EventTime { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>
        /// The stack trace.
        /// </value>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>
        /// The type of the exception.
        /// </value>
        public string ExceptionType { get; set; }

        #endregion

        #region ICloneable members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public LogEntry Clone()
        {
            // create a shallow copy of the object (deep copy not needed)
            return (LogEntry)MemberwiseClone();
        }

        #endregion
    }
}
