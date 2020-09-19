using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppTimerService.Utils
{
    /**
     * https://gist.github.com/sparksbat/38d3a8c31f36d18cc497831631691067
     */
    public class ProcessHelper
    {
        private readonly ILogger<Worker> _logger;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public ProcessHelper(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public Process GetForegroundProcess()
        {
            IntPtr hWnd;
            try
            {
                hWnd = GetForegroundWindow(); // Get foreground window handle
            } 
            catch (Exception e)
            {
                _logger.LogError($"Failed to retrieve foreground window process: {e.Message}");
                return null;
            }
            return GetWindowPtrProcess(hWnd);
        }

        private Process GetWindowPtrProcess(IntPtr hWnd)
        {
            uint processID = 0;
            try
            {
                GetWindowThreadProcessId(hWnd, out processID); // Get PID from window handle
                Process fgProc = Process.GetProcessById(Convert.ToInt32(processID)); // Get it as a C# obj.
                                                                                     // NOTE: In some rare cases ProcessID will be NULL. Handle this how you want. 
                return fgProc;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get process by window handle id [{hWnd.ToString()}]: {e.Message}");
                throw;
            }
        }
    }
}
