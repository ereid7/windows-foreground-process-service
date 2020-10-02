using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimerService.Contracts.Loggers
{
    public interface IForegroundProcessHistoryLogger
    {
        void LogInformation(int newProcessId,
                            string newProcessName,
                            int lastProcessId,
                            string lastProcessName,
                            string lastProcessDuration);
    }
}
