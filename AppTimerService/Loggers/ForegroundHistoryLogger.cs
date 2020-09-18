using AppTimerService.Managers;
using AppTimerService.Models;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace AppTimerService.Loggers
{
    public class ForegroundHistoryLogger : DirectoryManager
    {
        private readonly ILogger<Worker> _logger;
        public ForegroundHistoryLogger(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public void LogInformation(int newProcessId,
                                   string newProcessName,
                                   int lastProcessId,
                                   string lastProcessName,
                                   string lastProcessDuration)
        {
            var foregroundHistoryLog = new ForegroundHistoryLog(DateTime.Now,
                                                                newProcessId,
                                                                newProcessName,
                                                                lastProcessId,
                                                                lastProcessName,
                                                                lastProcessDuration);

            var records = new List<ForegroundHistoryLog> { foregroundHistoryLog };

            using (FileStream fs = new FileStream($"{_dailyDataPath}\\ForegroundLogs.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            using (StreamReader sr = new StreamReader(fs))
            using (StreamWriter sw = new StreamWriter(fs))
            using (var csvReader = new CsvReader(sr, CultureInfo.InvariantCulture))
            using (var csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture))
            {

                while (csvReader.Read()) { }

                csvWriter.WriteRecord(foregroundHistoryLog);
                csvWriter.NextRecord();
            }
        }
    }
}
