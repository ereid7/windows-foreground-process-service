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

namespace AppTimerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDictionary<int, TrackedProcessInfo> _processIds;
        private readonly DirectoryManager _directoryManager;

        private ForegroundProcessManager _foregroundHistoryManager;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _processIds = new Dictionary<int, TrackedProcessInfo>();
            _directoryManager = new DirectoryManager();
            _foregroundHistoryManager = new ForegroundProcessManager(_logger);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //// TODO initialize trackedP rocessInfo - 
            //// https://stackoverflow.com/questions/32590428/how-can-i-get-the-process-name-of-the-current-active-window-in-windows-with-wina
            while (!stoppingToken.IsCancellationRequested)
            {

                // foreground tracking
                _foregroundHistoryManager.UpdateForegroundProcess();

                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
