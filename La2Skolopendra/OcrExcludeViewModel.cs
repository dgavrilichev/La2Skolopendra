using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using CommonLibrary.Wpf;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class OcrExcludeViewModel : ViewModelBase
    {
        [NotNull] public ObservableCollection<OcrRemovableSelectorViewModel> RemovableSelectors { get; } = new ObservableCollection<OcrRemovableSelectorViewModel>();




        public ICommand AddExcludeRegionCommand
        {
            get
            {
                return new RelayCommand(o => { CreateNewSelector(); });
            }
        }

        private void CreateNewSelector()
        {
            var newSelector = new OcrRemovableSelectorViewModel();
            newSelector.RequestRemove += (sender, args) =>
            {
                RemovableSelectors.Remove(newSelector);
                Redraw();
            };
            newSelector.AreaBoundsChanged += NewSelectorOnAreaBoundsChanged;
            RemovableSelectors.Add(newSelector);
        }

        private void NewSelectorOnAreaBoundsChanged(object sender, Rectangle e)
        {
            

        }

        private void Redraw()
        {
            

        }
    }
}
