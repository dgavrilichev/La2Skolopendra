using System;
using System.Drawing;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using La2Bot;
// ReSharper disable InconsistentNaming

namespace La2Skolopendra.Native
{
    internal static class WindowCommandHelper
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        [UsedImplicitly]
        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            [UsedImplicitly]
            public int left;
            [UsedImplicitly]
            public int top;
            [UsedImplicitly]
            public int right;
            [UsedImplicitly]
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        internal static void LeftClick(IntPtr hWnd, Point point)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, point.X, point.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, point.X, point.Y, 0, 0);
        }
    }
}
