using System;
using System.Runtime.InteropServices;

namespace Com.CurtisRutland.WpfHotkeys.Win32
{
    public static class User32
    {
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
