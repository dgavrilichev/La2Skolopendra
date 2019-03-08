namespace CommonLibrary.Logging
{
    /// <summary>
    /// Writes information in log.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Suicide move detected.
        /// </summary>
        void Critical(string message);

        /// <summary>
        /// Bad, but we keep living with that.
        /// </summary>
        void Warning(string message);

        /// <summary>
        /// At least read this, thanks.
        /// </summary>
        void Important(string message);

        /// <summary>
        /// Nothing interesting.
        /// </summary>
        void Info(string message);

        /// <summary>
        /// Like it can help me anyway :C
        /// </summary>
        void Debug(string message);
    }
}
