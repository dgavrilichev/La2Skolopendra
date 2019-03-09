using System;
using System.Collections.ObjectModel;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class MainViewModel : ViewModelBase
    {
        internal event EventHandler RequestActivateWindow;
        private void OnRequestActivateWindow()
        {
            RequestActivateWindow?.Invoke(this, EventArgs.Empty);
        }

        [NotNull] private readonly ILogger _logger;

        [NotNull] public MainTabViewModel MainTabViewModel { get; }
        [NotNull] public OcrRegionViewModel OcrRegionViewModel { get; }

        internal MainViewModel([NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            MainTabViewModel = new MainTabViewModel(logger);
            MainTabViewModel.RequestActivateWindow += (sender, args) => OnRequestActivateWindow();
            
            OcrRegionViewModel = new OcrRegionViewModel();
        }
    }
}
