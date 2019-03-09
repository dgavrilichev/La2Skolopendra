using System.Windows.Media.Imaging;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    public sealed class OcrRegionViewModel : ViewModelBase
    {
        private BitmapSource _mainWindowImage;

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

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyPropertyChanged();
            }
        }

        internal void SetMainWindowImage(BitmapSource mainWindowImage)
        {
            IsEnabled = mainWindowImage != null;

            _mainWindowImage = mainWindowImage;

            if(mainWindowImage != null)
                Image = mainWindowImage;
        }
    }
}
