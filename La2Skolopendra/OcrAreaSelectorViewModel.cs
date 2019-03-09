using System;
using System.Drawing;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    public sealed class OcrAreaSelectorViewModel : ViewModelBase
    {
        internal event EventHandler<Rectangle> AreaBoundsChanged;
        private void OnAreaBoundsChanged(Rectangle e)
        {
            AreaBoundsChanged?.Invoke(this, e);
        }

        internal OcrAreaSelectorViewModel(Size imageSize)
        {
            MaxHeight = imageSize.Height;
            MaxWidth = imageSize.Width;
        }

        public int MaxHeight { get; }
        public int MaxWidth { get; }

        private int _currentX;
        public int CurrentX
        {
            get => _currentX;
            set
            {
                _currentX = value;
                OnAreaBoundsChanged(new Rectangle(CurrentX, CurrentY, CurrentWidth, CurrentHeight));
                NotifyPropertyChanged();
            }
        }

        private int _currentY;
        public int CurrentY
        {
            get => _currentY;
            set
            {
                _currentY = value;
                OnAreaBoundsChanged(new Rectangle(CurrentX, CurrentY, CurrentWidth, CurrentHeight));
                NotifyPropertyChanged();
            }
        }

        private int _currentHeight;
        public int CurrentHeight
        {
            get => _currentHeight;
            set
            {
                if (value < 1) value = 1;

                _currentHeight = value;
                OnAreaBoundsChanged(new Rectangle(CurrentX, CurrentY, CurrentWidth, CurrentHeight));
                NotifyPropertyChanged();
            }
        }

        private int _currentWidth;
        public int CurrentWidth
        {
            get => _currentWidth;
            set
            {
                if (value < 1) value = 1;

                _currentWidth = value;
                OnAreaBoundsChanged(new Rectangle(CurrentX, CurrentY, CurrentWidth, CurrentHeight));
                NotifyPropertyChanged();
            }
        }
    }
}
