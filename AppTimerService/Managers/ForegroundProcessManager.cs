using AppTimerService.Loggers;
using AppTimerService.Models;
using AppTimerService.Repositories;
using AppTimerService.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AppTimerService.Managers
{
    // TODO save end time on process end
    class ForegroundProcessManager 
    {
        private ForegroundProcessHistoryLogger _foregroundHistoryLogger;
        private ForegroundProcessInfoRepository _foregroundInfoRepository;
        private readonly ILogger<Worker> _logger;
        private Process _foregroundProcess;
        private ProcessHelper _processHelper;

        // directory paths
        private readonly string _dataPath;
        private string _dailyDataPath;

        private readonly List<int> _ignoredProcessIds;
        private readonly List<string> _ignoredProcessNames;

        /**
         * The moment of time that the current foreground
         * process came into foreground  
         */
        private DateTime _processForegroundTime;

        public string DailyDataPath
        {
            get { return _dailyDataPath; }
            set
            {
                _dailyDataPath = value;

                InitializeDailyDataDirectory();
                InitializeForegoundProcessHistoryLogger();
            }
        }

        public ForegroundProcessManager(ILogger<Worker> logger)
        {
            // TODO do not pass daily path, but compute each time something is logged.
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _dataPath = $"{programDataPath}\\ForegroundAppTracker";
            _dailyDataPath = $"{_dataPath}\\{DateTime.Today.ToString("dd-MM-yyyy")}";

            InitializeDataDirectory();
            InitializeDailyDataDirectory();

            _logger = logger;
            _processHelper = new ProcessHelper(_logger);
            InitializeForegoundProcessHistoryLogger();
            InitializeForegroundProcessInfoRepository();
        }

        // TODO have banned processids like "SearchUI" for windows menu
        // TODO have process setter with logic. Different method that gets foreground process
        public void UpdateForegroundProcess()
        {
            UpdateDailyDirectoryPath();

            var foregroundProcess = _processHelper.GetForegroundProcess();
            if (foregroundProcess == null) { return; }

            if (_foregroundProcess == null || 
               (foregroundProcess.Id != 0 && _foregroundProcess.Id != foregroundProcess.Id))
            {
                // update last
                var lastForegroundProcess = _foregroundProcess;
                if (lastForegroundProcess != null && _foregroundInfoRepository.GetById(lastForegroundProcess.Id) != null)
                {
                    UpdateForegroundProcessInfo(lastForegroundProcess);
                }

                // insert if new
                _foregroundProcess = foregroundProcess;
                if (_foregroundInfoRepository.GetById(_foregroundProcess.Id) == null)
                {
                    InsertForegroundProcessInfo(_foregroundProcess);
                }

                // log update
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

        private void InitializeDataDirectory()
        {
            Directory.CreateDirectory(_dataPath);
        }

        private void InitializeDailyDataDirectory()
        {
            Directory.CreateDirectory(_dailyDataPath);
        }

        // TODO create sepearate class for directory operations.
        private void UpdateDailyDirectoryPath()
        {
            var date = DateTime.ParseExact(_dailyDataPath.Substring(_dailyDataPath.Length - 10), "dd-MM-yyyy", null);
            if (date != DateTime.Today)
            {
                DailyDataPath = $"{_dataPath}\\{DateTime.Today.ToString("dd-MM-yyyy")}";
            }
        }

        private void InitializeForegroundProcessInfoRepository()
        {
            _foregroundInfoRepository = new ForegroundProcessInfoRepository(_dailyDataPath);
        }

        private void InitializeForegoundProcessHistoryLogger()
        {
            _foregroundHistoryLogger = new ForegroundProcessHistoryLogger(_logger, _dailyDataPath);
        }

        private void LogForegroundProcessUpdate(Process lastProcess, Process newProcess)
        {
            var duration = _processForegroundTime != null ? (DateTime.Now - _processForegroundTime): TimeSpan.FromSeconds(0);
            var newProcessName = string.IsNullOrEmpty(newProcess.MainWindowTitle) ? newProcess.ProcessName : newProcess.MainWindowTitle;
            var lastProcessName = string.IsNullOrEmpty(lastProcess.MainWindowTitle) ? lastProcess.ProcessName : lastProcess.MainWindowTitle;

            _foregroundHistoryLogger.LogInformation(newProcess.Id,
                                                    newProcessName,
                                                    lastProcess.Id,
                                                    lastProcessName,
                                                    duration.ToString(@"hh\:mm\:ss\:fff"));
        }
    }
}
