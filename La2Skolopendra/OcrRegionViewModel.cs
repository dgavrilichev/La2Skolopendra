using System.Drawing;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace La2Skolopendra
{
    public sealed class OcrRegionViewModel : ViewModelBase
    {
        [NotNull] private readonly OcrRegionInfo _ocrRegionInfo = new OcrRegionInfo
        {
            MyHp = new Rectangle(100, 200, 100, 10),
            TargetHp = new Rectangle(200, 200, 100, 10)
        };

        private readonly Brush _myHpColor = new SolidBrush(Color.FromArgb(127, 255, 127, 80));
        private readonly Brush _targetHpColor = new SolidBrush(Color.FromArgb(127, 100, 149, 237));
        private readonly Pen _borderPen = new Pen(Brushes.SpringGreen);
        
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

        internal OcrRegionViewModel()
        {
            TargetHpSelector = new OcrAreaSelectorViewModel();
            TargetHpSelector.AreaBoundsChanged += TargetHpSelectorOnAreaBoundsChanged;

            MyHpSelector = new OcrAreaSelectorViewModel();
            MyHpSelector.AreaBoundsChanged += MyHpSelectorOnAreaBoundsChanged;
        }

        internal void SetMainWindowImage(BitmapSource mainWindowImage)
        {
            IsEnabled = mainWindowImage != null;

            if (mainWindowImage != null)
            {
                Image = mainWindowImage;
                RegionImage = BitmapHelper.BitmapToBitmapSource(new Bitmap(Image.PixelWidth, Image.PixelHeight));

                MyHpSelector.MaxHeight = Image.PixelHeight;
                MyHpSelector.MaxWidth = Image.PixelWidth;

                TargetHpSelector.MaxHeight = Image.PixelHeight;
                TargetHpSelector.MaxWidth = Image.PixelWidth;

                RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
            }
        }

        private void MyHpSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            _ocrRegionInfo.MyHp = e;
            RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
        }

        private void TargetHpSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            _ocrRegionInfo.TargetHp = e;
            RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
        }

        [NotNull]
        private Bitmap DrawRegions()
        {
            var target = new Bitmap(Image.PixelWidth, Image.PixelHeight);
            target.MakeTransparent();

            using (var graphics = Graphics.FromImage(target))
            {
                graphics.FillRectangle(_myHpColor, _ocrRegionInfo.MyHp.X,
                    _ocrRegionInfo.MyHp.Y, 
                    _ocrRegionInfo.MyHp.Width,
                    _ocrRegionInfo.MyHp.Height);
                graphics.DrawRectangle(_borderPen, _ocrRegionInfo.MyHp.X,
                    _ocrRegionInfo.MyHp.Y,
                    _ocrRegionInfo.MyHp.Width,
                    _ocrRegionInfo.MyHp.Height);

                graphics.FillRectangle(_targetHpColor, _ocrRegionInfo.TargetHp.X,
                    _ocrRegionInfo.TargetHp.Y,
                    _ocrRegionInfo.TargetHp.Width,
                    _ocrRegionInfo.TargetHp.Height);
                graphics.DrawRectangle(_borderPen, _ocrRegionInfo.TargetHp.X,
                    _ocrRegionInfo.TargetHp.Y,
                    _ocrRegionInfo.TargetHp.Width,
                    _ocrRegionInfo.TargetHp.Height);
            }

            return target;
        }
    }
}
