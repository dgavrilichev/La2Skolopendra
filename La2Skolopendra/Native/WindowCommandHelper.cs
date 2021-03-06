﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using JetBrains.Annotations;
using La2Bot;
// ReSharper disable InconsistentNaming

namespace La2Skolopendra.Native
{
    internal static class WindowCommandHelper
    {
        internal enum KeyCodes
        {
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            Left = 0x25,
            Right = 0x27,
            Num1 = 0x61
        }

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const uint MK_LBUTTON = 0x0001;

        private const UInt32 WM_KEYDOWN = 0x0100;
        private const UInt32 WM_KEYUP = 0x0101;
        private const int VK_F5 = 0x74;
        private const int VK_RIGHT = 0x27;

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

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        internal static void PressKey(IntPtr hWnd, KeyCodes key)
        {
            PostMessage(hWnd, WM_KEYDOWN, (int)key, 0);
        }

        internal static void LeftClick(Point point)
        {
            var xpos = point.X;
            var ypos = point.Y;

            SetCursorPos(xpos, ypos);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
    }
}
