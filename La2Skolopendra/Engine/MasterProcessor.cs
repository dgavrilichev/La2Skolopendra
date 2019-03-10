using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using La2Skolopendra.Export;
using La2Skolopendra.Native;

namespace La2Skolopendra.Engine
{
    internal sealed class MasterProcessor
    {
        internal event EventHandler<string> Report;
        private void OnReport(string e)
        {
            Report?.Invoke(this, e);
        }

        internal event EventHandler<Bitmap> ScreenCapture;
        private void OnScreenCapture([NotNull] Bitmap e)
        {
            if(e == null) throw new ArgumentNullException(nameof(e));

            ScreenCapture?.Invoke(this, e);
        }

        private readonly IntPtr _hWnd;
        [NotNull] private readonly SkSettings _settings;

        internal MasterProcessor(IntPtr hWnd, [NotNull] SkSettings settings)
        {
            _hWnd = hWnd;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        internal async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                OnReport("Capture screen");
                var screenshot = ScreenshotHelper.GetScreenBitmap(_hWnd);
                await SelectGoodTarget(cancellationToken, screenshot);


                await Task.Delay(TimeSpan.FromSeconds(0.1), cancellationToken);
            }
        }

        private async Task SelectGoodTarget(CancellationToken cancellationToken, [NotNull] Bitmap screenshot)
        {
            if(screenshot == null) throw new ArgumentNullException(nameof(screenshot));

            var screenshotWithExclude = ScreenshotHelper.ApplyExclude(_settings.ExcludeInfo.Data, screenshot);

            throw new NotImplementedException();
        }

 
    }
}
