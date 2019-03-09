using System;
using System.Drawing;
using JetBrains.Annotations;

namespace La2Skolopendra
{
    internal sealed class WindowInfo
    {
        internal WindowInfo(IntPtr hWnd, bool isMain, [CanBeNull] Bitmap image)
        {
            HWnd = hWnd;
            IsMain = isMain;
            Image = image;
        }

        [CanBeNull] internal Bitmap Image { get; }
        internal IntPtr HWnd { get; }
        internal bool IsMain { get; }
    }
}
