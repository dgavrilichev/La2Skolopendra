using System;
using System.Collections.ObjectModel;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class MainTabViewModel : ViewModelBase
    {
        [NotNull] private readonly ILogger _logger;

        [NotNull] public ObservableCollection<La2WindowViewModel> La2WindowsCollection { get; } = new ObservableCollection<La2WindowViewModel>();

        internal MainTabViewModel([NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        }
    }
}
