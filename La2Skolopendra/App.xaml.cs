using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using CommonLibrary.Logging;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [NotNull]
        private readonly ILogger _logger = new ThreadSafeLogger(new FileLogger(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DateTime.Today:yy-MM-dd}.txt")));

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _logger.Important("App start");
            var window = new MainWindow(new MainViewModel(_logger));
            window.Show();
        }

        private void AppOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Critical(e.Exception.ToString());
            MessageBox.Show(e.Exception.ToString(), "Critical error!", MessageBoxButton.OK, MessageBoxImage.Error);
            //e.Handled = true;
        }
    }
}
