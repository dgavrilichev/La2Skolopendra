using System.Drawing;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    internal sealed class OcrAreaSelectorViewModel : ViewModelBase
    {
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
                NotifyPropertyChanged();
            }
        }

        private int _currentHeight;
        public int CurrentHeight
        {
            get => _currentHeight;
            set
            {
                _currentHeight = value;
                NotifyPropertyChanged();
            }
        }

        private int _currentWidth;
        public int CurrentWidth
        {
            get => _currentWidth;
            set
            {
                _currentWidth = value;
                NotifyPropertyChanged();
            }
        }
    }
}
