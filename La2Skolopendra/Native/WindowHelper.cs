using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace La2Skolopendra.Native
{
    internal static class WindowHelper
    {
        [DllImport("User32.dll")]
        internal static extern int SetForegroundWindow(IntPtr hWnd);

        [NotNull]
        internal static IEnumerable<IntPtr> GetWindowByName(string name)
        {
            var hWnd = IntPtr.Zero;
            foreach (var pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(name))
                {
                    hWnd = pList.MainWindowHandle;
                    yield return hWnd;
                }
            }
        }

        internal static Rect GetWindowRect(IntPtr hWnd)
        {
            var rect = new WindowHelper.Rect();
            GetWindowRect(hWnd, ref rect);

            return rect;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        internal static bool SendMessage(IntPtr hWnd)
        {
            return SendMessage(hWnd, WM_SYSCOMMAND, SC_RESTORE, 0);
        }

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
        private static Int32 WM_SYSCOMMAND = 0x0112;
        private static Int32 SC_RESTORE = 0xF120;
    }
}
