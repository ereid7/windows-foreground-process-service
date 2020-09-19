using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppTimerService.Utils
{
    /**
     * https://gist.github.com/sparksbat/38d3a8c31f36d18cc497831631691067
     */
    public class ProcessUtils
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static Process GetForegroundProcess()
        {
            IntPtr hWnd = GetForegroundWindow(); // Get foreground window handle
            return GetWindowPtrProcess(hWnd);
        }

        public static Process GetWindowPtrProcess(IntPtr hWnd)
        {
            uint processID = 0;
            GetWindowThreadProcessId(hWnd, out processID); // Get PID from window handle
            Process fgProc = Process.GetProcessById(Convert.ToInt32(processID)); // Get it as a C# obj.
            // NOTE: In some rare cases ProcessID will be NULL. Handle this how you want. 
            return fgProc;
        }
    }
}
