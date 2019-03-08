using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
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
        [NotNull] private readonly SynchronizationContext _uiContext;

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
            _uiContext = SynchronizationContext.Current;
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand(o => Task.Run(() => LoadWindows())); }
        }

        private async void LoadWindows()
        {
            await LoadWindowsAsync();
        }

        private async Task LoadWindowsAsync()
        {
            UpdateIsEnabled = false;

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => {
                   La2WindowsCollection.Clear();
                }));

            _logger.Info("Now loading La2 windows..");
            var la2Windows = WindowHelper.GetWindowsByName(WindowName).ToList();

            foreach (var la2Window in la2Windows)
            {
                await Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => {
                        La2WindowsCollection.Add(new La2WindowViewModel(null, la2Window.Id.ToString()));
                    }));                
            }

            UpdateIsEnabled = true;
        }
    }
}
