using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Com.CurtisRutland.WpfHotkeys.Win32;

namespace Com.CurtisRutland.WpfHotkeys
{
    /// <summary>
    /// WPF Hotkey Class. Must be instantiated in or after Window has been Loaded
    /// </summary>
    public class Hotkey : IDisposable
    {
        public event EventHandler<HotkeyEventArgs> HotkeyPressed;
        protected virtual void OnHotkeyPressed(HotkeyInfo info)
        {
            if (HotkeyPressed == null) return;
            HotkeyPressed(this, new HotkeyEventArgs(this, info));
        }

        public Modifiers Modifier { get; private set; }
        public Keys Key { get; set; }
        public int Id { get; private set; }

        private readonly IntPtr _hWnd;
        private bool _registered;

        public Hotkey(Modifiers modifier, Keys key, Window window, bool registerImmediately = false)
        {
            if (window == null) throw new ArgumentException("You must provide a form or window to register the hotkey to.", "window");
            Modifier = modifier;
            Key = key;
            _hWnd = new WindowInteropHelper(window).Handle;
            Id = GetHashCode();
            HookWndProc(window);
            if (registerImmediately) Register();
        }

        private void HookWndProc(Visual window)
        {
            var source = PresentationSource.FromVisual(window) as HwndSource;
            if (source == null) throw new HotkeyException("Could not create hWnd source from window.");
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var info = HotkeyInfo.GetFromMessage(msg, lParam);
            if (info != null)
            {
                if(info.Key == Key && info.Modifier == Modifier) OnHotkeyPressed(info);
            }
            return IntPtr.Zero;
        }

        public void Register()
        {
            if (_registered) return;
            if (!User32.RegisterHotKey(_hWnd, Id, (int)Modifier, (int)Key))
                throw new HotkeyException("Hotkey failed to register.");
            _registered = true;
        }

        public void Unregister()
        {
            if (!_registered) return;
            if (!User32.UnregisterHotKey(_hWnd, Id))
            {
                var wex = new Win32Exception();
                if (wex.NativeErrorCode != 0) throw new HotkeyException("Hotkey failed to unregister. See InnerException for details.", wex);
            }
            _registered = false;
        }

        public sealed override int GetHashCode()
        {
            return (int)Modifier ^ (int)Key ^ _hWnd.ToInt32();
        }

        public void Dispose()
        {
            Unregister();
            GC.SuppressFinalize(this);
        }

        ~Hotkey() { Unregister(); }
    }
}
