namespace FPS.Diagnostics
{
    /// <summary>
    /// Target of the logger.
    /// </summary>
    public interface ILoggerTarget
    {
        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="entry">Log entry.</param>
        void WriteLogEntry(LogEntry entry);
    }
}
