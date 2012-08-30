using System;

namespace FPS.Diagnostics
{
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="logType">Type of the log.</param>
        void Log(Exception exception, LogType logType);

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        void Log(string message, LogType logType);

        /// <summary>
        /// Logs the specified messageto EventLog.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <param name="logType">Type of the log.</param>
        void Log(string message, string source, LogType logType);

        /// <summary>
        /// Logs the specified messageto EventLog.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source type.</param>
        /// <param name="logType">Type of the log.</param>
        void Log(string message, SourceType source, LogType logType);
    }
}
