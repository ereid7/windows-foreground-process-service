using AppTimerService.Loggers;
using AppTimerService.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AppTimerService.Managers
{
    class ForegroundHistoryManager : DirectoryManager
    {
        private ForegroundHistoryLogger _foregroundHistoryLogger;
        private readonly ILogger<Worker> _logger;
        private Process _foregroundProcess;

        private readonly List<int> _ignoredProcessIds;
        private readonly List<string> _ignoredProcessNames;

        /**
         * The moment of time that the current foreground
         * process came into foreground
         */
        private DateTime _processForegroundTime;

        public ForegroundHistoryManager(ILogger<Worker> logger)
        {
            _logger = logger;
            // TODO do not pass daily path, but compute each time something is logged.
            _foregroundHistoryLogger = new ForegroundHistoryLogger(_logger);
        }

        // TODO have banned processids like "SearchUI" for windows menu
        public void UpdateForegroundProcess()
        {
            var foregroundProcess = ProcessUtils.GetForegroundProcess();
            if (_foregroundProcess == null || 
                (foregroundProcess.Id != 0 && _foregroundProcess?.Id != foregroundProcess.Id))
            {
                var lastForegroundProcess = _foregroundProcess;
                _foregroundProcess = foregroundProcess;

                if (lastForegroundProcess != null)
                {
                    LogForegroundProcessUpdate(lastForegroundProcess, _foregroundProcess);
                }
                _processForegroundTime = DateTime.Now;
            }
        }

        private void LogForegroundProcessUpdate(Process lastProcess, Process newProcess)
        {
            // TODO format duration
            var duration = _processForegroundTime != null ? (DateTime.Now - _processForegroundTime).TotalSeconds : 0;
            var newProcessName = string.IsNullOrEmpty(newProcess.MainWindowTitle) ? newProcess.ProcessName : newProcess.MainWindowTitle;
            var lastProcessName = string.IsNullOrEmpty(lastProcess.MainWindowTitle) ? lastProcess.ProcessName : lastProcess.MainWindowTitle;

            _foregroundHistoryLogger.LogInformation(newProcess.Id,
                                                    newProcessName,
                                                    lastProcess.Id,
                                                    lastProcessName,
                                                    duration.ToString());
        }
    }
}
