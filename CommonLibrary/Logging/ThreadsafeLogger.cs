using System;
using JetBrains.Annotations;

namespace CommonLibrary.Logging
{
    public sealed class ThreadSafeLogger : ILogger
    {
        [NotNull] private readonly ILogger _source;
        [NotNull] private readonly object _lock = new object();

        public ThreadSafeLogger([NotNull] ILogger source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public void Critical(string message)
        {
            lock(_lock) _source.Critical(message);
        }

        public void Warning(string message)
        {
            lock (_lock) _source.Warning(message);
        }

        public void Important(string message)
        {
            lock (_lock) _source.Important(message);
        }

        public void Info(string message)
        {
            lock (_lock) _source.Info(message);
        }

        public void Debug(string message)
        {
            lock (_lock) _source.Debug(message);
        }
    }
}
