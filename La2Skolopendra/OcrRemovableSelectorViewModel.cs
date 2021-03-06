﻿using System;
using System.Drawing;
using System.Windows.Input;
using CommonLibrary.Wpf;

namespace La2Skolopendra
{
    internal sealed class OcrRemovableSelectorViewModel : ViewModelBase
    {
        internal event EventHandler AreaBoundsChanged;
        private void OnAreaBoundsChanged()
        {
            AreaBoundsChanged?.Invoke(this, EventArgs.Empty);
        }

        internal event EventHandler RequestRemove;
        private void OnRequestRemove()
        {
            RequestRemove?.Invoke(this, EventArgs.Empty);
        }

        public int Id { get; }

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

        internal OcrRemovableSelectorViewModel(int id, Rectangle rectangle)
        {
            Id = id;
            SelectorViewModel = new OcrAreaSelectorViewModel(rectangle);
            SelectorViewModel.AreaBoundsChanged += (sender, rect) => OnAreaBoundsChanged();
        }

        public ICommand RemoveCommand
        {
            get { return new RelayCommand(o => OnRequestRemove()); }
        }
    }
}
