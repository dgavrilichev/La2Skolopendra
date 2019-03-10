using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace La2Skolopendra.Native
{
    internal static class ScreenshotHelper
    {
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [NotNull]
        public static Bitmap GetScreenBitmap(IntPtr hWnd)
        {
            var rect = WindowHelper.GetWindowRect(hWnd);
            var width = rect.right - rect.left;
            var height = rect.bottom - rect.top;
            // Create a bitmap to draw the capture into
            var bitmap = new Bitmap(width, height);           
            // Use PrintWindow to draw the window into our bitmap
            using (var g = Graphics.FromImage(bitmap))
            {
                var hdc = g.GetHdc();
                PrintWindow(hWnd, hdc, 0);
                g.ReleaseHdc(hdc);
            }

            return bitmap;
        }

        [NotNull]
        public static Bitmap ApplyExclude([NotNull] List<Rectangle> excludeInfoData, [NotNull] Bitmap src)
        {
            if(excludeInfoData == null) throw new ArgumentNullException(nameof(excludeInfoData));
            if(src == null) throw new ArgumentNullException(nameof(src));

            using (var scrGraphics = Graphics.FromImage(src))
            {
                var result = new Bitmap(src.Width, src.Height, scrGraphics);
                using (var resultGraphics = Graphics.FromImage(result))
                {
                    foreach (var rectangle in excludeInfoData)
                    {
                        resultGraphics.FillRectangle(Brushes.Black, rectangle);
                    }
                }

                return result;
            }
        }
    }
}
