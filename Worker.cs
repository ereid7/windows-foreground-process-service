using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppTimerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HashSet<int> _processIds;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _processIds = new HashSet<int>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now); 
                Process[] processes = Process.GetProcesses();

                foreach (Process p in processes)
                {
                    if (!String.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        if (!_processIds.Contains(p.Id))
                        {
                            _logger.LogInformation(p.MainWindowTitle);
                            _logger.LogInformation(p.StartTime.ToString());
                            _logger.LogInformation(p.Id.ToString());
                            _processIds.Add(p.Id);
      
                        }
                    }
                }

                foreach (int p in _processIds.ToList())
                {
                    var process = Process.GetProcessById(p);
                    var timeOpen = DateTime.Now.Subtract(process.StartTime);
                    _logger.LogInformation($"{process.MainWindowTitle}: {timeOpen.Duration()}");
                }



                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
