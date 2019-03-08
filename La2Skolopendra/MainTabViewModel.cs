using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonLibrary.Logging;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Skolopendra.Native;

namespace La2Skolopendra
{
    internal sealed class MainTabViewModel : ViewModelBase
    {
        private const string WindowName = "Lineage II";

        [NotNull] private readonly ILogger _logger;

        [NotNull] public ObservableCollection<La2WindowViewModel> La2WindowsCollection { get; } = new ObservableCollection<La2WindowViewModel>();

        private bool _updateIsEnabled = true;
        public bool UpdateIsEnabled
        {
            get => _updateIsEnabled;
            set
            {
                if (value == _updateIsEnabled) return;
                _updateIsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        internal MainTabViewModel([NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand(o => Task.Run(LoadWindows)); }
        }

        private async Task LoadWindows()
        {
            UpdateIsEnabled = false;

            _logger.Info("Now loading La2 windows..");
            var la2Windows = WindowHelper.GetWindowsByName(WindowName).ToList();
            await Task.Delay(TimeSpan.FromSeconds(3));


            UpdateIsEnabled = true;
        }
    }
}
