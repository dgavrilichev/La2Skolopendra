using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Skolopendra.Export;

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
        [NotNull] private readonly SkSettings _skSettings = new SkSettings();

        [NotNull] public MainTabViewModel MainTabViewModel { get; }
        [NotNull] public OcrRegionViewModel OcrRegionViewModel { get; }
        [NotNull] public OcrExcludeViewModel OcrExcludeViewModel { get; }

        private MasterViewModel _masterViewModel;
        public MasterViewModel MasterViewModel
        {
            get => _masterViewModel;
            set
            {
                _masterViewModel = value;
                NotifyPropertyChanged();
            }
        }

        internal MainViewModel([NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _skSettings.Load();

            MainTabViewModel = new MainTabViewModel(logger);
            MainTabViewModel.RequestActivateWindow += (sender, args) => OnRequestActivateWindow();
            MainTabViewModel.WindowsReload += OnWindowsReload;
            
            OcrRegionViewModel = new OcrRegionViewModel(_skSettings.RegionInfo);
            OcrRegionViewModel.OcrRegionUpdate += OnOcrRegionUpdate;
            OcrExcludeViewModel = new OcrExcludeViewModel(_skSettings.ExcludeInfo);
            OcrExcludeViewModel.RegionUpdated += OnRegionUpdated;
        }

        private void OnRegionUpdated(object sender, OcrExcludeInfo e)
        {
            _skSettings.ExcludeInfo = e;
            _skSettings.Save();
        }

        private void OnOcrRegionUpdate(object sender, OcrRegionInfo e)
        {
            _skSettings.RegionInfo = e;
            _skSettings.Save();
        }

        private void OnWindowsReload(object sender, [NotNull] List<WindowInfo> e)
        {
            if(e == null) throw new ArgumentNullException(nameof(e));

            var main = e.SingleOrDefault(w => w.IsMain);
            OcrRegionViewModel.SetMainWindowImage(main?.Image);
            OcrExcludeViewModel.SetMainWindowImage(main?.Image);
            var slave = e.FirstOrDefault(w => !w.IsMain);
            Debug.WriteLine(slave?.HWnd);
            if (main != null)
            {
                MasterViewModel = new MasterViewModel(_skSettings, main.HWnd, slave?.HWnd);
            }
        }
    }
}
