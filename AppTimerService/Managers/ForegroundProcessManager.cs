using AppTimerService.Loggers;
using AppTimerService.Models;
using AppTimerService.Repositories;
using AppTimerService.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AppTimerService.Managers
{
    // TODO save end time on process end
    class ForegroundProcessManager : DirectoryManager
    {
        private ForegroundProcessHistoryLogger _foregroundHistoryLogger;
        private ForegroundProcessInfoRepository _foregroundInfoRepository;
        private readonly ILogger<Worker> _logger;
        private Process _foregroundProcess;
        private ProcessHelper _processHelper;

        private readonly List<int> _ignoredProcessIds;
        private readonly List<string> _ignoredProcessNames;

        private Dictionary<int, ForegroundProcessInfo> _processMap;

        /**
         * The moment of time that the current foreground
         * process came into foreground  
         */
        private DateTime _processForegroundTime;

        public ForegroundProcessManager(ILogger<Worker> logger)
        {
            // TODO do not pass daily path, but compute each time something is logged.
            _logger = logger;
            _processHelper = new ProcessHelper(_logger);
            _foregroundHistoryLogger = new ForegroundProcessHistoryLogger(_logger, _dailyDataPath);
            _processMap = new Dictionary<int, ForegroundProcessInfo>();
            InitializeForegroundProcessInfoRepository();
        }

        // TODO have banned processids like "SearchUI" for windows menu
        // TODO have process setter with logic. Different method that gets foreground process
        public void UpdateForegroundProcess()
        {
            var foregroundProcess = _processHelper.GetForegroundProcess();
            if (foregroundProcess == null) { return; }

            if (_foregroundProcess == null || 
               (foregroundProcess.Id != 0 && _foregroundProcess.Id != foregroundProcess.Id))
            {
                // update last
                var lastForegroundProcess = _foregroundProcess;
                // insert if new
                _foregroundProcess = foregroundProcess;
                if (_foregroundInfoRepository.GetById(_foregroundProcess.Id) == null)
                {
                    // create entity item, add, and update repository.
                    // _foregroundInfoRepository.AddItem();
                }

                // updateprocessmap

                if (lastForegroundProcess != null)
                {
                    LogForegroundProcessUpdate(lastForegroundProcess, _foregroundProcess);
                }
                _processForegroundTime = DateTime.Now;
            }
        }

        private void InitializeForegroundProcessInfoRepository()
        {
            _foregroundInfoRepository = new ForegroundProcessInfoRepository(_dailyDataPath);
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
