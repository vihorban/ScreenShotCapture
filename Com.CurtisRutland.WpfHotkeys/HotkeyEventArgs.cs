namespace Com.CurtisRutland.WpfHotkeys
{
    public class HotkeyEventArgs
    {
        public HotkeyInfo HotkeyInfo { get; private set; }
        public Hotkey Hotkey { get; private set; }

        public HotkeyEventArgs(Hotkey hotkey, HotkeyInfo info)
        {
            HotkeyInfo = info;
            Hotkey = hotkey;
        }
    }
}

