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
        internal static IEnumerable<IntPtr> GetWindowsByName(string windowName)
        {
            if(string.IsNullOrEmpty(windowName)) throw new ArgumentNullException(nameof(windowName));

            return from pList in Process.GetProcesses()
                where pList.MainWindowTitle.Contains(windowName)
                select pList.MainWindowHandle;
        }
    }
}
