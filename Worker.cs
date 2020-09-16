using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppTimerService.Managers;
using AppTimerService.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppTimerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDictionary<int, TrackedProcessInfo> _processIds;
        private readonly DirectoryManager _directoryManager;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _processIds = new Dictionary<int, TrackedProcessInfo>();
            _directoryManager = new DirectoryManager();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO initialize trackedP rocessInfo - 
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                Process[] processes = Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (!String.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        //_logger.LogInformation(p.MainWindowTitle);
                        //_logger.LogInformation(p.StartTime.ToString());
                        //_logger.LogInformation(p.Id.ToString());

                        if (!_processIds.ContainsKey(p.Id))
                        {
                            var trackedProcessInfo = new TrackedProcessInfo(p.Id.ToString(), p.MainWindowTitle, "test");
                            _processIds.Add(p.Id, trackedProcessInfo);

                            _logger.LogInformation($"Process {p.MainWindowTitle} started at {p.StartTime}");

                            p.EnableRaisingEvents = true;
                            p.Exited += (s, e) =>
                            {
                                _processIds.Remove(p.Id);
                                _logger.LogInformation($"Process {p.MainWindowTitle} stopped at {p.ExitTime}");
                                this.updateDataStore();
                            };

                            this.updateDataStore();
                        }
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private void updateDataStore()
        {
            _logger.LogInformation($"Data store updated");
        }
    }
}
