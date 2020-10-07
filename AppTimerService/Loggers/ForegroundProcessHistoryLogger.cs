using AppTimerService.Models;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;

namespace AppTimerService.Loggers
{
    public class ForegroundProcessHistoryLogger
    {
        private readonly ILogger<Worker> _logger;
        private string _directoryPath;
        public ForegroundProcessHistoryLogger(ILogger<Worker> logger, string directoryPath)
        {
            _logger = logger;
            _directoryPath = directoryPath;
        }

        public void LogInformation(int newProcessId,
                                   string newProcessName,
                                   int lastProcessId,
                                   string lastProcessName,
                                   string lastProcessDuration)
        {
            // TODO update console logger
            _logger.LogInformation($"{newProcessName}, {lastProcessName}");
            var csvPath = $"{_directoryPath}\\ForegroundLogs.csv";

            try
            {
                var foregroundHistoryLog = new ForegroundProcessHistoryLog(DateTime.Now,
                                                                newProcessId,
                                                                newProcessName,
                                                                lastProcessId,
                                                                lastProcessName,
                                                                lastProcessDuration);

                using (FileStream fs = new FileStream($"{_directoryPath}\\ForegroundLogs.csv", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
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
            catch (Exception e)
            {
                _logger.LogError(e, $"Error writing foreground process history log at {csvPath}");
            }
        }
    }
}
