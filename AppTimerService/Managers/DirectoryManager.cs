using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppTimerService.Managers
{
    public class DirectoryManager
    {
        // directory paths
        private readonly string _dataPath;
        private string _dailyDataPath;
        private DateTime _currentDate;

        public string DailyDataPath
        {
            get { return _dailyDataPath; }
            set
            {
                _dailyDataPath = value;

                InitializeDailyDataDirectory();
            }
        }

        public DirectoryManager(ILogger<Worker> logger)
        {
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _currentDate = DateTime.Today;
            _dataPath = $"{programDataPath}\\ForegroundAppTracker";
            _dailyDataPath = $"{_dataPath}\\{_currentDate.ToString("dd-MM-yyyy")}";

            InitializeDataDirectory();
            InitializeDailyDataDirectory();
        }

        public void UpdateDailyDirectoryPath()
        {
            DateTime today = DateTime.Today;
            if (_currentDate != today)
            {
                _currentDate = today;
                DailyDataPath = $"{_dataPath}\\{_currentDate.ToString("dd-MM-yyyy")}";
            }
        }

        private void InitializeDataDirectory()
        {
            Directory.CreateDirectory(_dataPath);
        }

        private void InitializeDailyDataDirectory()
        {
            Directory.CreateDirectory(_dailyDataPath);
        }
    }
}
