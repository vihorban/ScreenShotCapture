using System;

namespace Com.CurtisRutland.WpfHotkeys
{
    public class HotkeyException : Exception
    {
        public HotkeyException(string message) : base(message) { }
        public HotkeyException(string message, Exception inner) : base(message, inner) { }
    }
}
