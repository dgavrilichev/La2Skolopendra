using System;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class MainViewModel : ViewModelBase
    {
        [NotNull] private readonly ILogger _logger;

        internal MainViewModel([NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
