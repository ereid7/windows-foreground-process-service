using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppTimerService.Managers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppTimerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HashSet<int> _processIds;
        private readonly DirectoryManager _directoryManager;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _processIds = new HashSet<int>();
            _directoryManager = new DirectoryManager();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

                        if (!_processIds.Contains(p.Id))
                        {
                            _processIds.Add(p.Id);

                            _logger.LogInformation($"Process {p.MainWindowTitle} started at {p.StartTime}");

                            p.EnableRaisingEvents = true;
                            p.Exited += (s, e) =>
                            {
                                _processIds.Remove(p.Id);
                                _logger.LogInformation($"Process {p.MainWindowTitle} stopped at {p.ExitTime}");
                            };
                        }
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
