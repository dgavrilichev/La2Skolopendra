using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using CommonLibrary;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Skolopendra.Native;

namespace La2Skolopendra
{
    internal sealed class MainTabViewModel : ViewModelBase
    {
        private const string WindowName = "Lineage II";

        internal event EventHandler RequestActivateWindow;
        private void OnRequestActivateWindow()
        {
            RequestActivateWindow?.Invoke(this, EventArgs.Empty);
        }

        [NotNull] private readonly ILogger _logger;

        [NotNull] public ObservableCollection<La2WindowViewModel> La2WindowsCollection { get; } = new ObservableCollection<La2WindowViewModel>();

        private bool _updateIsEnabled = true;

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
                var screenshot = ScreenshotHelper.GetScreenBitmap(la2Window.handle);
                var source = BitmapHelper.BitmapToBitmapSource(screenshot);
                source.Freeze();
                La2WindowsCollection.Add(new La2WindowViewModel(source, la2Window.id));
                Thread.Sleep(100);
            }

            OnRequestActivateWindow();
            UpdateIsEnabled = true;
        }
    }
}
