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


        private readonly string _processId;

        private BitmapSource _image;
        public BitmapSource Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyPropertyChanged();
            }
        }

        internal IntPtr HWnd { get; }
        internal bool IsMain { get; private set; }

        private bool _windowIsEnabled = true;
        public bool WindowIsEnabled
        {
            get => _windowIsEnabled;
            set
            {
                if (value == _windowIsEnabled) return;
                _windowIsEnabled = value;

                if (!value)
                    SetAsMainEnabled = false;
                else
                    SetAsMainEnabled = !IsMain;

                OnIsEnabledChanged(value);
                NotifyPropertyChanged();
            }
        }

        private bool _canBeDisabled = true;
        public bool CanBeDisabled
        {
            get => _canBeDisabled;
            set
            {
                if (value == _canBeDisabled) return;
                _canBeDisabled = value;
                NotifyPropertyChanged();
            }
        }

        private bool _setAsMainEnabled = true;
        public bool SetAsMainEnabled
        {
            get => _setAsMainEnabled;
            set
            {
                if (value == _setAsMainEnabled) return;
                _setAsMainEnabled = value;
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

        internal La2WindowViewModel([NotNull] BitmapSource bitmapSource, string processId, IntPtr hWnd)
        {
            if(string.IsNullOrEmpty(processId)) throw new ArgumentNullException(processId);
            Image = bitmapSource ?? throw new ArgumentNullException(nameof(bitmapSource));
            _processId = processId;
            HWnd = hWnd;
            SetAsSlave();
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
            IsMain = true;
            SetAsMainEnabled = false;
            CanBeDisabled = false;
            ShowProcessId();
        }

        internal void SetAsSlave()
        {
            IsMain = false;
            SetAsMainEnabled = true;
            CanBeDisabled = true;
            ShowProcessId();
        }

        private void ShowProcessId()
        {
            ProcessIdDisplay = $"{_processId} {(IsMain ? "(main)" : string.Empty)}";
        }
    }
}
