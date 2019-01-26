using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UX365Core
{
    public class WindowHelper
    {
        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                User32.EnumThreadWindows((uint)thread.Id,
                    (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        public static List<Window> GetAvailableWindows()
        {
            List<Window> AvailableWindows = new List<Window>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName.ToLower() == "explorer")
                        continue;

                    if (Properties.Config.OnlyUX365Control && process.ProcessName.ToLower() != "ux365control")
                        continue;

                    foreach (var handle in WindowHelper.EnumerateProcessWindowHandles(process.Id))
                    {
                        Window wnd = new Window(handle, process);
                        wnd.SetSkin(Properties.Config.UsedSkin);
                        AvailableWindows.Add(wnd);
                    }
                }
                catch
                {
                }
            }
            return AvailableWindows;
        }
    }
}
