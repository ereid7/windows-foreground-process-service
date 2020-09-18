using AppTimerService.Loggers;
using AppTimerService.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AppTimerService.Managers
{
    class ForegroundHistoryManager : DirectoryManager
    {
        private ForegroundHistoryLogger _foregroundHistoryLogger;
        private readonly ILogger<Worker> _logger;
        private Process _foregroundProcess;

        /**
         * The moment of time that the current foreground
         * process came into foreground
         */
        private DateTime _processForegroundTime;

        public ForegroundHistoryManager(ILogger<Worker> logger)
        {
            _logger = logger;
            // TODO do not patah daily path, but compute each time something is logged.
            _foregroundHistoryLogger = new ForegroundHistoryLogger(_logger);
        }

        public void UpdateForegroundProcess()
        {
            var foregroundProcess = ProcessUtils.getForegroundProcess();
            if (_foregroundProcess?.Id != foregroundProcess.Id)
            {
                var lastForegroundProcess = _foregroundProcess;
                _foregroundProcess = foregroundProcess;

                // TODO add starting log
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
            _foregroundHistoryLogger.LogInformation(newProcess.Id,
                                                    newProcess.MainWindowTitle,
                                                    lastProcess.Id,
                                                    lastProcess.MainWindowTitle,
                                                    duration.ToString());
        }
    }
}
