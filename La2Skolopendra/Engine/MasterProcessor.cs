using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using La2Skolopendra.Export;
using La2Skolopendra.Native;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Tesseract;
using BitmapHelper = CommonLibrary.BitmapHelper;
using Point = System.Drawing.Point;

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
                WindowHelper.Restore(_hWnd);
                WindowHelper.SetForegroundWindow(_hWnd);

                OnReport("Capture screen");
                var screenshot = ScreenshotHelper.GetScreenBitmap(_hWnd);

                var targetIsSelected = await SelectGoodTarget(cancellationToken, screenshot);

                if (targetIsSelected)
                    await FightTarget(cancellationToken);
                else
                    await TurnScreen(cancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            }
        }

        private async Task TurnScreen(CancellationToken cancellationToken)
        {

        }

        private async Task FightTarget(CancellationToken cancellationToken)
        {
            OnReport($"Target found!");
            while (!cancellationToken.IsCancellationRequested)
            {
                var screenshot = ScreenshotHelper.GetScreenBitmap(_hWnd);
                var targetHealth = ScreenshotHelper.GetSubPart(screenshot, _settings.RegionInfo.TargetHp);
                var targetHealthPercent = GetTargetHpPercent(targetHealth);
                OnReport($"Target health: {targetHealthPercent:###.##}%");
                if(Math.Abs(targetHealthPercent) < 0.00001)
                    break;
                
                WindowCommandHelper.PressKey(_hWnd, WindowCommandHelper.KeyCodes.F1);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }

            OnReport($"Target is dead!");
            WindowCommandHelper.PressKey(_hWnd, WindowCommandHelper.KeyCodes.F4);
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            WindowCommandHelper.PressKey(_hWnd, WindowCommandHelper.KeyCodes.F4);
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            WindowCommandHelper.PressKey(_hWnd, WindowCommandHelper.KeyCodes.F4);
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            WindowCommandHelper.PressKey(_hWnd, WindowCommandHelper.KeyCodes.F4);
        }

        private async Task<bool> SelectGoodTarget(CancellationToken cancellationToken, [NotNull] Bitmap screenshot)
        {
            if(screenshot == null) throw new ArgumentNullException(nameof(screenshot));

            var screenshotWithExclude = ScreenshotHelper.ApplyExclude(_settings.ExcludeInfo.Data, screenshot);
            OnScreenCapture(new Bitmap(screenshotWithExclude));

            var src = Mat.FromImageData(BitmapHelper.ImageToByte2(screenshotWithExclude));
            var blacked = new Mat();
            Cv2.Threshold(src, blacked, 127, 255, ThresholdTypes.Binary);

            var blackedBitmap = blacked.ToBitmap();

            var targets = GetTargets(blackedBitmap);
            foreach (var target in targets)
            {
                WindowCommandHelper.LeftClick(target);
                var screenshotWithHp = ScreenshotHelper.GetScreenBitmap(_hWnd);
                var targetHealth = ScreenshotHelper.GetSubPart(screenshotWithHp, _settings.RegionInfo.TargetHp);
                var targetHealthPercent = GetTargetHpPercent(targetHealth);
                OnReport($"Target health: {targetHealthPercent}");

                if (targetHealthPercent > 0)               
                    return true;          

                OnReport($"Target is dead or neutral");
                await Task.Delay(TimeSpan.FromSeconds(0.5), cancellationToken);
            }

            return false;
        }

        private double GetTargetHpPercent(Bitmap hpBar)
        {
            //r 99-231
            //g 0-73
            //b 0-132
            var y = hpBar.Height / 2;
            double currentHp = 0;
            for (var x = 0; x < hpBar.Width; x++)
            {
                var isRed = IsRedHp(hpBar.GetPixel(x, y));
                if (isRed)                
                    currentHp += 1;                
                else               
                    break;                
            }

            return currentHp / hpBar.Width * 100.0;
        }

        private bool IsRedHp(Color color)
        {
            return color.R > 200 && color.G < 100 && color.B < 140;
        }

        private List<Point> GetTargets([NotNull] Bitmap blacked)
        {
            if(blacked == null) throw new ArgumentNullException(nameof(blacked));

            var result = new List<Point>();
            var log = string.Empty;
            var windowRect = WindowHelper.GetWindowRect(_hWnd);

            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var page = engine.Process(blacked))
                {
                    using (var iterator = page.GetIterator())
                    {
                        iterator.Begin();
                        do
                        {
                            if (iterator.TryGetBoundingBox(PageIteratorLevel.TextLine, out var rect))
                            {
                                var curText = iterator.GetText(PageIteratorLevel.TextLine);
                                var modifiedString = curText.Replace("\n", "");

                                if (modifiedString.Length > 4 && rect.Height > 3 && rect.Height < 20)
                                {
                                    log = $"{log}\n{modifiedString} | {rect.ToString()}";

                                    var targetPoint = new Point((rect.X1 + rect.X2) / 2 + windowRect.left,
                                        rect.Y1 + 30 + windowRect.top);
                                    result.Add(targetPoint);
                                }
                            }
                        } while (iterator.Next(PageIteratorLevel.TextLine));
                    }
                }
            }

            log = result.Any() 
                ? $"Found possible targets ({result.Count}): {log}" 
                : $"Targets not found";
            OnReport(log);

            return result;
        }
    }
}
