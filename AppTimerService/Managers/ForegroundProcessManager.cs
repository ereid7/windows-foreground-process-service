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

        //private Dictionary<int, ForegroundProcessInfo> _processMap;

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
                if (lastForegroundProcess != null && _foregroundInfoRepository.GetById(lastForegroundProcess.Id) != null)
                {
                    // TODO track last foregroundtime in info
                    UpdateForegroundProcessInfo(lastForegroundProcess);
                }

                // insert if new
                _foregroundProcess = foregroundProcess;
                if (_foregroundInfoRepository.GetById(_foregroundProcess.Id) == null)
                {
                    _foregroundProcess.Exited += (sender, e) =>
                    {
                        UpdateProcessExitTime(_foregroundProcess);
                    };

                    InsertForegroundProcessInfo(_foregroundProcess);
                }

                if (lastForegroundProcess != null)
                {
                    LogForegroundProcessUpdate(lastForegroundProcess, _foregroundProcess);
                }
                _processForegroundTime = DateTime.Now;
            }
        }

        private void InsertForegroundProcessInfo(Process p)
        {
            // create entity item, add, and update repository.
            var processInfo = new ForegroundProcessInfoEntity();
            processInfo.Initialize(p);
            _foregroundInfoRepository.AddItem(processInfo);
            _foregroundInfoRepository.SaveChanges();
        }

        private void UpdateForegroundProcessInfo(Process p)
        {
            var processInfo = _foregroundInfoRepository.GetById(p.Id);
            var foregroundDuration = TimeSpan.Parse(processInfo.ForegroundDuration);
            foregroundDuration += (DateTime.Now - _processForegroundTime);
            processInfo.ForegroundDuration = foregroundDuration.ToString();
            _foregroundInfoRepository.UpdateItem(processInfo);
            _foregroundInfoRepository.SaveChanges();
        }

        private void UpdateProcessExitTime(Process p)
        {
            var processInfo = _foregroundInfoRepository.GetById(p.Id);
            processInfo.EndTime = p.ExitTime.ToString();
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
