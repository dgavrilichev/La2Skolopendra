using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Skolopendra.Export;

namespace La2Skolopendra
{
    internal sealed class OcrExcludeViewModel : ViewModelBase
    {
        internal event EventHandler<OcrExcludeInfo> RegionUpdated;
        private void OnRegionUpdated()
        {
            var info = new OcrExcludeInfo();
            foreach (var selector in RemovableSelectors)
            {
                info.Data.Add(new Rectangle(selector.SelectorViewModel.CurrentX,
                    selector.SelectorViewModel.CurrentY,
                    selector.SelectorViewModel.CurrentWidth,
                    selector.SelectorViewModel.CurrentHeight));
            }

            RegionUpdated?.Invoke(this, info);
        }

        [NotNull] private readonly Pen _borderPen = new Pen(Brushes.SpringGreen);
        [NotNull] private readonly Font _drawFont = new Font("Arial", 16);

        private int _currentId;

        [NotNull] public ObservableCollection<OcrRemovableSelectorViewModel> RemovableSelectors { get; } = new ObservableCollection<OcrRemovableSelectorViewModel>();

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

        public ICommand AddExcludeRegionCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    CreateNewSelector(new Rectangle(100, 100, 200, 50));
                    RegionImage = DrawRegions();
                    OnRegionUpdated();
                });
            }
        }

        private void CreateNewSelector(Rectangle rectangle)
        {
            var newSelector = new OcrRemovableSelectorViewModel(_currentId, rectangle);
            _currentId++;
            newSelector.RequestRemove += (sender, args) =>
            {
                RemovableSelectors.Remove(newSelector);
                RegionImage = DrawRegions();
            };
            newSelector.AreaBoundsChanged += OnSelectorAreaBoundsChanged;
            RemovableSelectors.Add(newSelector);
        }

        private bool _isEnabled;

        public OcrExcludeViewModel([NotNull] OcrExcludeInfo skSettingsExcludeInfo)
        {
            if(skSettingsExcludeInfo == null) throw new ArgumentNullException(nameof(skSettingsExcludeInfo));

            foreach (var rectangle in skSettingsExcludeInfo.Data)
            {
                CreateNewSelector(rectangle);
            }
        }

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

            if (mainWindowImage != null)
            {
                Image = mainWindowImage;

                foreach (var selector in RemovableSelectors)
                {
                    selector.SelectorViewModel.MaxHeight = Image.PixelHeight;
                    selector.SelectorViewModel.MaxWidth = Image.PixelWidth;
                }

                RegionImage = DrawRegions();
            }
        }

        private void OnSelectorAreaBoundsChanged(object sender, EventArgs e)
        {
            RegionImage = DrawRegions();
            OnRegionUpdated();
        }

        [NotNull]
        private BitmapSource DrawRegions()
        {
            var target = new Bitmap(Image.PixelWidth, Image.PixelHeight);
            target.MakeTransparent();

            using (var graphics = Graphics.FromImage(target))
            {
                foreach (var selector in RemovableSelectors)
                {
                    graphics.FillRectangle(Brushes.Black, selector.SelectorViewModel.CurrentX,
                        selector.SelectorViewModel.CurrentY,
                        selector.SelectorViewModel.CurrentWidth,
                        selector.SelectorViewModel.CurrentHeight);

                    graphics.DrawRectangle(_borderPen, selector.SelectorViewModel.CurrentX,
                        selector.SelectorViewModel.CurrentY,
                        selector.SelectorViewModel.CurrentWidth,
                        selector.SelectorViewModel.CurrentHeight);

                    graphics.DrawString(selector.Id.ToString(), _drawFont, Brushes.DarkOrange, 
                        selector.SelectorViewModel.CurrentX + selector.SelectorViewModel.CurrentWidth / 2,
                        selector.SelectorViewModel.CurrentY + selector.SelectorViewModel.CurrentHeight / 2 - 10);
                }
            }

            return BitmapHelper.BitmapToBitmapSource(target);
        }
    }
}
