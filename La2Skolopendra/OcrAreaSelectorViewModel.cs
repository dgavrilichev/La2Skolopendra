using System;
using System.Drawing;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    public sealed class OcrAreaSelectorViewModel : ViewModelBase
    {
        internal event EventHandler AreaBoundsChanged;
        private void OnAreaBoundsChanged()
        {
            AreaBoundsChanged?.Invoke(this, EventArgs.Empty);
        }

        internal OcrAreaSelectorViewModel()
        {
            MaxHeight = 1000;
            MaxWidth = 2000;
        }

        private int _maxWidth;
        public int MaxWidth
        {
            get => _maxWidth;
            set
            {
                _maxWidth = value;
                NotifyPropertyChanged();
            }
        }

        private int _maxHeight;
        public int MaxHeight
        {
            get => _maxHeight;
            set
            {
                _maxHeight = value;
                NotifyPropertyChanged();
            }
        }

        private int _currentX = 200;
        public int CurrentX
        {
            get => _currentX;
            set
            {
                _currentX = value;
                OnAreaBoundsChanged();
                NotifyPropertyChanged();
            }
        }

        private int _currentY = 100;
        public int CurrentY
        {
            get => _currentY;
            set
            {
                _currentY = value;
                OnAreaBoundsChanged();
                NotifyPropertyChanged();
            }
        }

        private int _currentHeight = 30;
        public int CurrentHeight
        {
            get => _currentHeight;
            set
            {
                if (value < 1) value = 1;

                _currentHeight = value;
                OnAreaBoundsChanged();
                NotifyPropertyChanged();
            }
        }

        private int _currentWidth = 200;
        public int CurrentWidth
        {
            get => _currentWidth;
            set
            {
                if (value < 1) value = 1;

                _currentWidth = value;
                OnAreaBoundsChanged();
                NotifyPropertyChanged();
            }
        }
    }
}
