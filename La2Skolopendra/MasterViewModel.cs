using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommonLibrary;
using CommonLibrary.Wpf;
using JetBrains.Annotations;
using La2Skolopendra.Engine;
using La2Skolopendra.Export;

namespace La2Skolopendra
{
    internal sealed class MasterViewModel : ViewModelBase
    {
        [NotNull] private readonly MasterProcessor _masterProcessor;

        internal MasterViewModel([NotNull] SkSettings settings, IntPtr hWnd)
        {
            if(settings == null) throw new ArgumentNullException(nameof(settings));

            _masterProcessor = new MasterProcessor(hWnd, settings);
            _masterProcessor.ScreenCapture += MasterProcessorOnScreenCapture;
            _masterProcessor.Report += MasterProcessorOnReport;
        }

        private void MasterProcessorOnReport(object sender, string e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Log.Length > 1000)
                {
                    var ln = Log.IndexOf("\n", StringComparison.Ordinal);
                    Log = $"{Log.Substring(ln)}\n[{DateTime.Now:T}] {e}";
                }
                else
                {
                    Log = $"{Log}\n[{DateTime.Now:T}] {e}";
                }
            });
        }

        private async void Start()
        {
            using (var cts = new CancellationTokenSource())
            {
                await Task.Run(()=> _masterProcessor.Run(cts.Token), cts.Token);
            }
        }

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

        private string _log = string.Empty;
        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                NotifyPropertyChanged();
            }
        }

        private void MasterProcessorOnScreenCapture(object sender, Bitmap e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Image = BitmapHelper.BitmapToBitmapSource(e);
            });
        }

        public ICommand StartCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    Start();
                });
            }
        }
    }
}
