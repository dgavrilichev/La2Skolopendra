using System;
using System.Diagnostics;
using System.IO;

namespace CommonLibrary.Logging
{
    /// <inheritdoc />
    public sealed class FileLogger : ILogger
    {
        private readonly string _filePath;

        private static string Date => DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss.fffffff");

        public FileLogger(string filePath)
        {
            if(string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            _filePath = filePath;
        }

        /// <inheritdoc />
        public void Critical(string message)
        {
            WriteToFile($"{Date} |S| !!! ERROR !!! | {message}");
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            WriteToFile($"{Date} |A| ... WARNING ... | {message}");
        }

        /// <inheritdoc />
        public void Important(string message)
        {
            WriteToFile($"{Date} |B| IMPORTANT | {message}");
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            WriteToFile($"{Date} |C| info | {message}");
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            WriteToFile($"{Date} |D| debug | {message}");
        }

        private void WriteToFile(string message)
        {
            using (var fileWriter = File.AppendText(_filePath))
            {
                try
                {
                    fileWriter.WriteLine(message);
                    fileWriter.Flush();
                    fileWriter.Close();
                }
                catch (Exception e)
                {
                    using (var eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "LineageCore";
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error);
                        eventLog.WriteEntry(message, EventLogEntryType.Warning);
                    }
                }
            }
        }
    }
}
