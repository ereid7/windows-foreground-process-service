using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppTimerService.Managers;
using AppTimerService.Models;
using AppTimerService.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// TODO remove interfaces. Unneded without unity
namespace AppTimerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ForegroundProcessManager _foregroundHistoryManager;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _foregroundHistoryManager = new ForegroundProcessManager(_logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // TODO log startup time
                // TODO config file for custom file save location, custom names, etc

                // foreground tracking
                _foregroundHistoryManager.UpdateForegroundProcess();

                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
