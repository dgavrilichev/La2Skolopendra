using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    public sealed class La2WindowViewModel : ViewModelBase
    {
        internal event EventHandler<bool> IsEnabledChanged;
        private void OnIsEnabledChanged(bool e)
        {
            IsEnabledChanged?.Invoke(this, e);
        }

        internal event EventHandler SetAsMain;
        private void OnSetAsMain()
        {
            SetAsMain?.Invoke(this, EventArgs.Empty);
        }

        private readonly IntPtr _hWnd;
        private string _processId;
        private bool _isMain;

        public BitmapSource Image { get; }
 

        private bool _windowIsEnabled = true;
        public bool WindowIsEnabled
        {
            get => _windowIsEnabled;
            set
            {
                if (value == _windowIsEnabled) return;
                _windowIsEnabled = value;
                OnIsEnabledChanged(value);
                NotifyPropertyChanged();
            }
        }

        private string _processIdDisplay;
        public string ProcessIdDisplay
        {
            get => _processIdDisplay;
            set
            {
                if (value == _processIdDisplay) return;
                _processIdDisplay = value;
                NotifyPropertyChanged();
            }
        }

        internal La2WindowViewModel([NotNull] BitmapSource bitmapSource, string processIdDisplay, IntPtr hWnd)
        {
            if(string.IsNullOrEmpty(processIdDisplay)) throw new ArgumentNullException(processIdDisplay);
            Image = bitmapSource ?? throw new ArgumentNullException(nameof(bitmapSource));
            ProcessIdDisplay = processIdDisplay;
            _hWnd = hWnd;
        }

        public ICommand SetAsMainCommand
        {
            get
            {
                return new RelayCommand(o => { SetAsMainAction(); });
            }
        }

        internal void SetAsMainAction()
        {
            OnSetAsMain();
            _isMain = true;
            ShowProcessId();
        }


        private void ShowProcessId()
        {
            ProcessIdDisplay = $"{_processId} {(_isMain ? "(main)" : string.Empty)}";
        }
    }
}
