using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class OcrExcludeViewModel : ViewModelBase
    {
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
                return new RelayCommand(o => { CreateNewSelector(); });
            }
        }

        private void CreateNewSelector()
        {
            var newSelector = new OcrRemovableSelectorViewModel(_currentId);
            _currentId++;
            newSelector.RequestRemove += (sender, args) =>
            {
                RemovableSelectors.Remove(newSelector);
                RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
            };
            newSelector.AreaBoundsChanged += NewSelectorOnAreaBoundsChanged;
            RemovableSelectors.Add(newSelector);
            RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
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

            if (mainWindowImage != null)
            {
                Image = mainWindowImage;
                RegionImage = BitmapHelper.BitmapToBitmapSource(new Bitmap(Image.PixelWidth, Image.PixelHeight));

                foreach (var selector in RemovableSelectors)
                {
                    selector.SelectorViewModel.MaxHeight = Image.PixelHeight;
                    selector.SelectorViewModel.MaxWidth = Image.PixelWidth;
                }

                RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
            }
        }

        private void NewSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            RegionImage = BitmapHelper.BitmapToBitmapSource(DrawRegions());
        }

        [NotNull]
        private Bitmap DrawRegions()
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
                            selector.SelectorViewModel.CurrentY + selector.SelectorViewModel.CurrentHeight / 2);
                }
            }

            return target;
        }
    }
}
