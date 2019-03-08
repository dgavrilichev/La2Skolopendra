using System;

namespace CommonLibrary.Logging
{
    /// <inheritdoc />
    public class ConsoleLogger : ILogger
    {
        private static string Date => DateTime.Now.ToString("MM.dd.yyyy HH:mm:ss.fffffff");

        /// <inheritdoc />
        public void Critical(string message)
        {
            WriteToConsole(message, ConsoleColor.Red);
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            WriteToConsole(message, ConsoleColor.Yellow);
        }

        /// <inheritdoc />
        public void Important(string message)
        {
            WriteToConsole(message, ConsoleColor.Blue);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            WriteToConsole(message, ConsoleColor.White);
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            WriteToConsole(message, ConsoleColor.DarkGray);
        }

        private static void WriteToConsole(string message, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"[{Date}] ");
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
