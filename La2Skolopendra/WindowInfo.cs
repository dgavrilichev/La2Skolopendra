using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class WindowInfo
    {
        internal WindowInfo(IntPtr hWnd, bool isMain, [CanBeNull] BitmapSource image)
        {
            HWnd = hWnd;
            IsMain = isMain;
            Image = image;
        }

        [CanBeNull] internal BitmapSource Image { get; }
        internal IntPtr HWnd { get; }
        internal bool IsMain { get; }
    }
}
