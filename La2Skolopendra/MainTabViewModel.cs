﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        internal event EventHandler<List<(IntPtr hWnd, bool isMain)>> UpdateWindowTabs;
        private void OnUpdateWindowTabs([NotNull] List<(IntPtr hWnd, bool isMain)> e)
        {
            if(e == null) throw new ArgumentNullException(nameof(e));

            UpdateWindowTabs?.Invoke(this, e);
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
          
            _logger.Info("Now loading La2 windows..");

            var existedWindows = new List<La2WindowViewModel>();
            var la2Windows = WindowHelper.GetWindowByName(WindowName);

            foreach (var la2Window in la2Windows)
            {
                var screenshot = ScreenshotHelper.GetScreenBitmap(la2Window.handle);
                var source = BitmapHelper.BitmapToBitmapSource(screenshot);
                source.Freeze();

                var existed = La2WindowsCollection.SingleOrDefault(w => w.HWnd == la2Window.handle);
                if (existed == null)
                {
                    var viewModel = new La2WindowViewModel(source, la2Window.id, la2Window.handle);
                    viewModel.IsEnabledChanged += WindowOnIsEnabledChanged;
                    viewModel.SetAsMain += WindowOnSetAsMain;
                    La2WindowsCollection.Add(viewModel);
                }
                else
                {
                    existed.Image = source;
                    existedWindows.Add(existed);
                }

                Thread.Sleep(100);
            }

            var closed = La2WindowsCollection.Where(w => !existedWindows.Contains(w)).ToList();
            foreach (var la2WindowViewModel in closed)
            {
                La2WindowsCollection.Remove(la2WindowViewModel);
            }

            OnRequestActivateWindow();
            UpdateIsEnabled = true;
        }

        private void WindowOnSetAsMain(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WindowOnIsEnabledChanged(object sender, bool e)
        {
            throw new NotImplementedException();
        }
    }
}
