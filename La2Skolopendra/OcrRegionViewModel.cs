using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;

namespace La2Skolopendra
{
    public sealed class OcrRegionViewModel : ViewModelBase
    {
        [NotNull] private readonly OcrRegionInfo _ocrRegionInfo = new OcrRegionInfo();

        private readonly Brush _myHpColor = new SolidBrush(Color.FromArgb(127, 255, 127, 80));
        private readonly Brush _targetHpColor = new SolidBrush(Color.FromArgb(127, 100, 149, 237));

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
                RegionImage = BitmapHelper.BitmapToBitmapSource(new Bitmap(Image.PixelWidth, Image.PixelHeight));
                TargetHpSelector = new OcrAreaSelectorViewModel(new Size(mainWindowImage.PixelWidth, mainWindowImage.PixelHeight));
                TargetHpSelector.AreaBoundsChanged += TargetHpSelectorOnAreaBoundsChanged;
                MyHpSelector = new OcrAreaSelectorViewModel(new Size(mainWindowImage.PixelWidth, mainWindowImage.PixelHeight));
                MyHpSelector.AreaBoundsChanged += MyHpSelectorOnAreaBoundsChanged;
            }
        }

        private void MyHpSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            _ocrRegionInfo.MyHp = e;
            RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRectangle());
        }

        private void TargetHpSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            _ocrRegionInfo.TargetHp = e;
            RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRectangle());
        }

        [NotNull]
        private Bitmap DrawRectangle()
        {
            var target = new Bitmap(Image.PixelWidth, Image.PixelHeight);
            target.MakeTransparent();

            using (var graphics = Graphics.FromImage(target))
            {
                graphics.FillRectangle(_myHpColor, _ocrRegionInfo.MyHp.X,
                    _ocrRegionInfo.MyHp.Y, 
                    _ocrRegionInfo.MyHp.Width,
                    _ocrRegionInfo.MyHp.Height);

                graphics.FillRectangle(_targetHpColor, _ocrRegionInfo.MyHp.X,
                    _ocrRegionInfo.MyHp.Y,
                    _ocrRegionInfo.MyHp.Width,
                    _ocrRegionInfo.MyHp.Height);
            }

            return target;
        }
    }
}
