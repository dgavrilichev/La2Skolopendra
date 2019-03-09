using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    public sealed class OcrRegionViewModel : ViewModelBase
    {
        [NotNull] private readonly OcrRegionInfo _ocrRegionInfo = new OcrRegionInfo();

        private readonly Brush _myHpColor = Brushes.Coral;
        private readonly Brush _targetHpColor = Brushes.CornflowerBlue;

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

        private BitmapSource _regionImage;
        public BitmapSource RegionImage
        {
            get => _regionImage;
            set
            {
                _regionImage = value;
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

        private OcrAreaSelectorViewModel _targetHpSelector;
        public OcrAreaSelectorViewModel TargetHpSelector
        {
            get => _targetHpSelector;
            set
            {
                _targetHpSelector = value;
                NotifyPropertyChanged();
            }
        }

        private OcrAreaSelectorViewModel _myHpSelector;
        public OcrAreaSelectorViewModel MyHpSelector
        {
            get => _myHpSelector;
            set
            {
                _myHpSelector = value;
                NotifyPropertyChanged();
            }
        }

        internal void SetMainWindowImage(BitmapSource mainWindowImage)
        {
            IsEnabled = mainWindowImage != null;

            _mainWindowImage = mainWindowImage;

            if (mainWindowImage != null)
            {
                Image = mainWindowImage;
                TargetHpSelector = new OcrAreaSelectorViewModel(new Size(mainWindowImage.PixelWidth, mainWindowImage.PixelHeight));
                TargetHpSelector.AreaBoundsChanged += TargetHpSelectorOnAreaBoundsChanged;
                MyHpSelector = new OcrAreaSelectorViewModel(new Size(mainWindowImage.PixelWidth, mainWindowImage.PixelHeight));
                MyHpSelector.AreaBoundsChanged += MyHpSelectorOnAreaBoundsChanged;
            }
        }

        private void MyHpSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {

            _ocrRegionInfo.MyHp = e;

        }

        private void TargetHpSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            _ocrRegionInfo.TargetHp = e;
        }

        [NotNull]
        private Bitmap DrawRectangle(Rectangle rectangle, Brush color, [NotNull] Bitmap srcBitmap)
        {
            if(srcBitmap == null) throw new ArgumentNullException(nameof(srcBitmap));

            return null;
        }
    }
}
