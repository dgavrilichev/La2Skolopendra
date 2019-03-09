using System;
using System.Windows.Media.Imaging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    public sealed class La2WindowViewModel : ViewModelBase
    {
        public BitmapSource Image { get; }
        public string ProcessId { get; }

        private bool _windowIsEnabled = true;
        public bool WindowIsEnabled
        {
            get => _windowIsEnabled;
            set
            {
                if (value == _windowIsEnabled) return;
                _windowIsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private bool _windowIsMain = true;
        public bool WindowIsMain
        {
            get => _windowIsMain;
            set
            {
                if (value == _windowIsMain) return;
                _windowIsMain = value;
                NotifyPropertyChanged();
            }
        }

        internal La2WindowViewModel([NotNull] BitmapSource bitmapSource, string processId)
        {
            if(string.IsNullOrEmpty(processId)) throw new ArgumentNullException(processId);
            Image = bitmapSource ?? throw new ArgumentNullException(nameof(bitmapSource));
            ProcessId = processId;
        }
    }
}
