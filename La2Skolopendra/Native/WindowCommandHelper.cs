using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using La2Bot;
// ReSharper disable InconsistentNaming

namespace La2Skolopendra.Native
{
    internal static class WindowCommandHelper
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;

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
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        internal static async Task LeftClick(IntPtr hWnd, Point point)
        {
            var lParam = MakeLParam(point.X, point.Y);
            PostMessage(hWnd, WM_LBUTTONDOWN, 0, lParam);
            await Task.Delay(TimeSpan.FromSeconds(0.2));
            PostMessage(hWnd, WM_LBUTTONUP, 0, lParam);
        }

        private static int MakeLParam(int LoWord, int HiWord)
        {
            return (HiWord << 16) | (LoWord & 0xFFFF);
        }

    }
}
