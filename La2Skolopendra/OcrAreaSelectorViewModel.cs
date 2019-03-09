using System.Drawing;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    internal sealed class OcrAreaSelectorViewModel : ViewModelBase
    {
        internal OcrAreaSelectorViewModel(Size imageSize)
        {
            
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
    }
}
