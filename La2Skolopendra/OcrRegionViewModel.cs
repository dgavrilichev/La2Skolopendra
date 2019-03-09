using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    public sealed class OcrRegionViewModel : ViewModelBase
    {
        private Bitmap _mainWindowImage;

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

        internal void SetMainWindowImage([NotNull] Bitmap mainWindowImage)
        {
            _mainWindowImage = mainWindowImage ?? throw new ArgumentNullException(nameof(mainWindowImage));
            Image = BitmapHelper.BitmapToBitmapSource(mainWindowImage);
        }
    }
}
