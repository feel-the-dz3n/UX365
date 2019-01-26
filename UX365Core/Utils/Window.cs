using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UX365Core
{
    public class Window
    {
        public Window(IntPtr WindowHandle, System.Diagnostics.Process relatedProcess)
        {
            Handle = WindowHandle;
            RelatedProcess = relatedProcess;
        }

        public Window(IntPtr WindowHandle, int relatedProcessId)
        {
            Handle = WindowHandle;
            RelatedProcess = System.Diagnostics.Process.GetProcessById(relatedProcessId);
        }

        public void SetSkin(WindowSkin skin)
        {
            if(Skin != null)
                Skin = null;

            Skin = skin;
        }

        private WindowSkin _skin = null;
        public WindowSkin Skin
        {
            get
            {
                return _skin;
            }
            private set
            {
                _skin = value;

                Draw.UpdateSkin(this);
            }
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();

            if (RelatedProcess != null)
                b.AppendLine($"Process: {RelatedProcess.ProcessName}[{RelatedProcess.Id}]");
            else
                b.AppendLine($"Process: null");

            if (Skin != null)
                b.AppendLine($"Skin: {Skin.SkinName}]");
            else
                b.AppendLine($"Skin: null");

            if (Visible)
            {
                b.AppendLine($"Window Text: {Title}");
                b.AppendLine($"Location: X: {Location.X}, Y: {Location.Y}");
                b.AppendLine($"Size: W: {Size.Width}, H: {Size.Height}");
            }
            b.AppendLine($"Visible: {Visible}");
            b.Append($"Handle: 0x{Handle.ToString("X4")}");
            return b.ToString();
        }

        public IntPtr Handle { get; set; }
        public System.Diagnostics.Process RelatedProcess { get; set; }

        public string Title
        {
            get
            {
                StringBuilder message = new StringBuilder(1000);
                User32.SendMessage(Handle, (int)User32.WM_GETTEXT, (IntPtr)message.Capacity, message);
                return message.ToString();
            }
        }

        public string Text { get { return Title; } }

        public User32.Rect WindowRect
        {
            get
            {
                User32.Rect WinPos = new User32.Rect();
                User32.GetWindowRect(Handle, ref WinPos);
                return WinPos;
            }
        }

        public bool Topmost
        {
            get
            {
                long style = (long)User32.GetWindowLongPtr(Handle, (int)User32.GWL.GWL_STYLE);

                if ((style & (uint)User32.WindowStyles.WS_EX_TOPMOST) == (uint)User32.WindowStyles.WS_EX_TOPMOST)
                {
                    return true;
                }

                return false;
            }
        }

        public System.Windows.WindowState WindowState
        {
            get
            {
                long style = (long)User32.GetWindowLongPtr(Handle, (int)User32.GWL.GWL_STYLE);

                if ((style & (uint)User32.WindowStyles.WS_MAXIMIZE) == (uint)User32.WindowStyles.WS_MAXIMIZE)
                {
                    return System.Windows.WindowState.Maximized;
                }
                else if ((style & (uint)User32.WindowStyles.WS_MINIMIZE) == (uint)User32.WindowStyles.WS_MINIMIZE)
                {
                    return System.Windows.WindowState.Minimized;
                }

                return System.Windows.WindowState.Normal;
            }
        }

        public bool Visible
        {
            get
            {
                return User32.IsWindowVisible(Handle);
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                var rect = WindowRect;
                return new System.Drawing.Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            }
        }

        public System.Drawing.Point Location
        {
            get
            {
                var rect = WindowRect;
                return new System.Drawing.Point(rect.Left, rect.Top);
            }
        }

    }
}
