using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Skolopendra.Export;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace La2Skolopendra
{
    public sealed class OcrRegionViewModel : ViewModelBase
    {
        internal event EventHandler<OcrRegionInfo> OcrRegionUpdate;
        private void OnOcrRegionUpdate(OcrRegionInfo e)
        {
            OcrRegionUpdate?.Invoke(this, e);
        }

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
            TargetHpSelector.AreaBoundsChanged += OnAreaBoundsChanged;

            MyHpSelector = new OcrAreaSelectorViewModel();
            MyHpSelector.AreaBoundsChanged += OnAreaBoundsChanged;
        }

        private void OnAreaBoundsChanged(object sender, EventArgs e)
        {
            RegionImage = DrawRegions();
            OnOcrRegionUpdate(new OcrRegionInfo
            {
                MyHp = new Rectangle(MyHpSelector.CurrentX,
                    MyHpSelector.CurrentY,
                    MyHpSelector.CurrentWidth,
                    MyHpSelector.CurrentHeight),
                TargetHp = new Rectangle(TargetHpSelector.CurrentX,
                    TargetHpSelector.CurrentY,
                    TargetHpSelector.CurrentWidth,
                    TargetHpSelector.CurrentHeight),
            });
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

                RegionImage = DrawRegions();
            }
        }

        [NotNull]
        private BitmapSource DrawRegions()
        {
            var target = new Bitmap(Image.PixelWidth, Image.PixelHeight);
            target.MakeTransparent();

            using (var graphics = Graphics.FromImage(target))
            {
                graphics.FillRectangle(_myHpColor, MyHpSelector.CurrentX,
                    MyHpSelector.CurrentY,
                    MyHpSelector.CurrentWidth,
                    MyHpSelector.CurrentHeight);
                graphics.DrawRectangle(_borderPen, MyHpSelector.CurrentX,
                    MyHpSelector.CurrentY,
                    MyHpSelector.CurrentWidth,
                    MyHpSelector.CurrentHeight);

                graphics.FillRectangle(_targetHpColor, TargetHpSelector.CurrentX,
                    TargetHpSelector.CurrentY,
                    TargetHpSelector.CurrentWidth,
                    TargetHpSelector.CurrentHeight);
                graphics.DrawRectangle(_borderPen, TargetHpSelector.CurrentX,
                    TargetHpSelector.CurrentY,
                    TargetHpSelector.CurrentWidth,
                    TargetHpSelector.CurrentHeight);
            }

            return BitmapHelper.BitmapToBitmapSource(target);
        }

    }
}
