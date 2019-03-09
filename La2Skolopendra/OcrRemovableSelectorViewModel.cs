using System;
using System.Drawing;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    internal sealed class OcrRemovableSelectorViewModel : ViewModelBase
    {
        internal event EventHandler<Rectangle> AreaBoundsChanged;
        private void OnAreaBoundsChanged(Rectangle e)
        {
            AreaBoundsChanged?.Invoke(this, e);
        }

        internal event EventHandler RequestRemove;
        private void OnRequestRemove()
        {
            RequestRemove?.Invoke(this, EventArgs.Empty);
        }

        private OcrAreaSelectorViewModel _selectorViewModel;
        public OcrAreaSelectorViewModel SelectorViewModel
        {
            get => _selectorViewModel;
            set
            {
                if (value == _selectorViewModel) return;
                _selectorViewModel = value;
                NotifyPropertyChanged();
            }
        }

        internal OcrRemovableSelectorViewModel()
        {
            SelectorViewModel = new OcrAreaSelectorViewModel();
            SelectorViewModel.AreaBoundsChanged += (sender, rectangle) => OnAreaBoundsChanged(rectangle);
        }
    }
}
