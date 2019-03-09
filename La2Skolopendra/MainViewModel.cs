using System;
using System.Collections.Generic;
using System.Linq;
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
            MainTabViewModel.WindowsReload += OnWindowsReload;
            
            OcrRegionViewModel = new OcrRegionViewModel();
        }

        private void OnWindowsReload(object sender, [NotNull] List<WindowInfo> e)
        {
            if(e == null) throw new ArgumentNullException(nameof(e));

            OcrRegionViewModel.SetMainWindowImage(e.Single(w => w.IsMain).Image);
        }
    }
}
