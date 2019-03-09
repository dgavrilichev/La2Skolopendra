using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommonLibrary;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Bot;
using La2Skolopendra.Native;

namespace La2Skolopendra
{
    internal sealed class MainTabViewModel : ViewModelBase
    {
        private const string WindowName = "Lineage II";

        [NotNull] private readonly ILogger _logger;

        [NotNull] public ObservableCollection<La2WindowViewModel> La2WindowsCollection { get; } = new ObservableCollection<La2WindowViewModel>();

        private bool _updateIsEnabled = true;
        [NotNull] private readonly SynchronizationContext _uiContext;

        public bool UpdateIsEnabled
        {
            get => _updateIsEnabled;
            set
            {
                if (value == _updateIsEnabled) return;
                _updateIsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        internal MainTabViewModel([NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uiContext = SynchronizationContext.Current;
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand(o => LoadWindows()); }
        }

        private void LoadWindows()
        {
            UpdateIsEnabled = false;
            La2WindowsCollection.Clear();
          
            _logger.Info("Now loading La2 windows..");

            var la2Windows = WindowHelper.GetWindowByName(WindowName);
            foreach (var la2Window in la2Windows)
            {
                var screenshot = ScreenshotHelper.GetScreenBitmap(la2Window);
                var source = BitmapHelper.BitmapToBitmapSource(screenshot);
                source.Freeze();
                La2WindowsCollection.Add(new La2WindowViewModel(source, "ss"));
                Thread.Sleep(2000);
            }

            UpdateIsEnabled = true;
        }
    }
}
