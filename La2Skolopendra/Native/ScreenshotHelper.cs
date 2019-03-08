﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using JetBrains.Annotations;

namespace La2Skolopendra.Native
{
    internal static class ScreenshotHelper
    {
        [NotNull]
        public static Bitmap GetScreenBitmap(IntPtr hWnd)
        {
            WindowHelper.SetForegroundWindow(hWnd);
            WindowHelper.SendMessage(hWnd);
            var rect = WindowHelper.GetWindowRect(hWnd);

            var width = rect.right - rect.left;
            var height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            return bmp;
        }
    }
}